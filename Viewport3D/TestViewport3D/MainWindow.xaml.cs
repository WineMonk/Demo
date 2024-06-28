using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.Wpf;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Viewport3DTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string modelPath1 = "D:\\file\\data\\test\\pointcloud\\20140406_sandy_usgs_de_1986.las";
            string mapPath = "D:\\file\\program\\test\\helix-toolkit\\Images\\EnvironmentMaps\\Cubemap_Grandcanyon.dds";
            SceneNodeGroupModel3D model3D = new SceneNodeGroupModel3D();
            Task.Run(() =>
            {
                var loader = new Importer();
                return loader.Load(modelPath1);
            }).ContinueWith((result) =>
            {
                if (result.IsCompleted)
                {
                    HelixToolkitScene scene = result.Result;
                    if (scene != null)
                    {
                        model3D.AddNode(scene.Root);
                    }
                }
                else if (result.IsFaulted && result.Exception != null)
                {
                    MessageBox.Show(result.Exception.Message);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            using (var file = new FileStream(mapPath, FileMode.Open))
            {
                var memory = new MemoryStream();
                file.CopyTo(memory);
                environmentMap3D.Texture = memory;
            }
            element3DPresenter.Content = model3D;
        }
    }
}