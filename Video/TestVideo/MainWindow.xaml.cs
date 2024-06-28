using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TestVideo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private bool isDraggingSlider = false;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            mediaElement.MediaOpened += MediaElement_MediaOpened;
            mediaElement.MediaEnded += MediaElement_MediaEnded;

        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                progressSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                timer.Start();
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            mediaElement.Position = TimeSpan.Zero;
            progressSlider.Value = 0;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDraggingSlider)
            {
                progressSlider.Value = mediaElement.Position.TotalSeconds;
            }
        }

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "视频文件 (*.mp4;*.avi;*.mkv)|*.mp4;*.avi;*.mkv|所有文件 (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mediaElement.Source = new Uri(openFileDialog.FileName);
                mediaElement.Play();
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
            timer.Stop();
        }

        private void BtnFastForward_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position + TimeSpan.FromSeconds(10);
        }

        private void BtnRewind_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position - TimeSpan.FromSeconds(10);
        }

        private void BtnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            mediaElement.Stretch = System.Windows.Media.Stretch.Fill;
        }

        private void BtnExitFullScreen_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            mediaElement.Stretch = System.Windows.Media.Stretch.Uniform;
        }

        private void SpeedComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (mediaElement != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)speedComboBox.SelectedItem;
                double speed = double.Parse(selectedItem.Content.ToString().Replace("x", ""));
                mediaElement.SpeedRatio = speed;
            }
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan && !isDraggingSlider)
            {
                mediaElement.Position = TimeSpan.FromSeconds(progressSlider.Value);
            }
        }

        private void ProgressSlider_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDraggingSlider = true;
        }

        private void ProgressSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDraggingSlider = false;
            mediaElement.Position = TimeSpan.FromSeconds(progressSlider.Value);
        }
    }
}