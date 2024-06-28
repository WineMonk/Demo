using Microsoft.Win32;
using OpenCvSharp;
using OpenCvSharp.Face;
using System.IO;
using System.Windows;

namespace TestOpenCV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        //人脸识别器
        FaceRecognizer _faceRecongnizer = FisherFaceRecognizer.Create();
        //人脸id，name字典
        Dictionary<int, string> _faceNameDic = new Dictionary<int, string>();
        //人脸数据统一大小
        OpenCvSharp.Size _imgSize = new OpenCvSharp.Size(1000, 1000);
        public MainWindow()
        {
            InitializeComponent();
            InitializeTrain();
        }

        private void InitializeTrain()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string faceDir = baseDir + "face\\";
            string[] faceImageDirs = Directory.GetDirectories(faceDir, "*_*");
            //读取人脸数据
            List<Mat> faceMats = new List<Mat>();
            List<int> faceIds = new List<int>();
            foreach (var faceImageDir in faceImageDirs)
            {
                string[] faceImages = Directory.GetFiles(faceImageDir, "*.jpg");
                if (faceImages.Length < 1)
                {
                    continue;
                }
                DirectoryInfo faceImageDirInfo = new DirectoryInfo(faceImageDir);
                string[] faceNameArr = faceImageDirInfo.Name.Split('_');
                int id = int.Parse(faceNameArr[0]);
                string name = faceNameArr[1];
                _faceNameDic.Add(id, name);
                IEnumerable<Mat> mats = faceImages.Select(face =>
                {
                    Mat mat = new Mat(face, ImreadModes.Grayscale);
                    Cv2.Resize(mat, mat, _imgSize);
                    return mat;
                });
                IEnumerable<int> ids = mats.Select(e => id);
                faceMats.AddRange(mats);
                faceIds.AddRange(ids);
            }
            //训练
            _faceRecongnizer.Train(faceMats, faceIds);
            //保存训练数据
            //_faceRecongnizer.Save("train.xml");
        }

        private void btnOpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg;*.png";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            Mat image = Cv2.ImRead(openFileDialog.FileName);
            Cv2.ImShow("image", image);
        }

        private void btnOpenVideo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "视频文件|*.mp4";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }
            // Opens MP4 file (ffmpeg is probably needed)
            VideoCapture capture = new VideoCapture(openFileDialog.FileName);

            int sleepTime = (int)Math.Round(1000 / capture.Fps);

            using (OpenCvSharp.Window window = new OpenCvSharp.Window("capture"))
            using (Mat image = new Mat()) // Frame image buffer
            {
                // When the movie playback reaches end, Mat.data becomes NULL.
                while (true)
                {
                    capture.Read(image); // same as cvQueryFrame
                    if (image.Empty())
                        break;

                    window.ShowImage(image);
                    Cv2.WaitKey(sleepTime);
                }
            }
        }

        private void btnCameraFaceRecognition_Click(object sender, RoutedEventArgs e)
        {
            //加载人眼、人脸模型数据
            CascadeClassifier faceFinder = new CascadeClassifier(@"haarcascade_frontalface_default.xml");
            CascadeClassifier eyeFinder = new CascadeClassifier(@"haarcascade_eye_tree_eyeglasses.xml");
            using (OpenCvSharp.Window window = new OpenCvSharp.Window("video - 按ESC退出"))
            //获取camera
            using (FrameSource video = Cv2.CreateFrameSource_Camera(0))
            using (Mat frame = new Mat())
            {
                while (true)
                {
                    //获取帧
                    video.NextFrame(frame);
                    //进行检测识别
                    OpenCvSharp.Rect[] faceRects = faceFinder.DetectMultiScale(frame);
                    OpenCvSharp.Rect[] eyeRects = eyeFinder.DetectMultiScale(frame);
                    //如果没有检测到人脸，就跳过
                    if (faceRects.Length < 1)
                    {
                        continue;
                    }
                    for (int i = 0; i < faceRects.Length; i++)
                    {
                        //人脸区域
                        OpenCvSharp.Rect rect = faceRects[i];
                        using (Mat nFrame = frame.Clone())
                        {
                            Mat m1 = new Mat(frame, rect);
                            Cv2.CvtColor(m1, m1, ColorConversionCodes.BGR2GRAY);
                            //设置大小
                            Cv2.Resize(m1, nFrame, _imgSize);
                            //人脸识别
                            _faceRecongnizer.Predict(nFrame, out int id, out double confidence);
                            //置信度
                            confidence = Math.Round(confidence, 2);
                            _faceNameDic.TryGetValue(id, out var name);
                            string label = name == null ? "unknow" : $"{name}  {confidence}";
                            // 在图像上绘制文字
                            Cv2.PutText(frame, label, new OpenCvSharp.Point(rect.Left, rect.Top - 10), HersheyFonts.HersheySimplex, 1.0, new Scalar(0, 0, 255), 2, LineTypes.Link8);
                        }
                        //绘制人脸框
                        Cv2.Rectangle(frame, faceRects[i], new Scalar(0, 0, 255), 1);
                    }
                    //眼部区域
                    if (eyeRects.Length > 1)
                    {
                        for (int i = 0; i < eyeRects.Length; i++)
                        {
                            //绘制眼部框
                            Cv2.Rectangle(frame, eyeRects[i], new Scalar(255, 0, 0), 1);
                        }
                    }
                    //显示结果
                    window.ShowImage(frame);
                    int v = Cv2.WaitKey(1);
                    //ESC - 27
                    if (v == 27)
                    {
                        break;
                    }
                }
            }
        }
    }
}