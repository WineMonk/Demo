using Mapsui.Layers;
using Mapsui.Nts.Providers.Shapefile;
using Mapsui.UI.Wpf;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestMapsui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShapeFile shapeFile = new ShapeFile("D:\\data\\TestData\\shp\\fw\\fw.shp");
            var layer = new Layer()
            {
                Name = "测试",
                DataSource = shapeFile,
            };
            mapControl.Map?.Layers.Add(layer);
            mapControl.Map.Navigator.RotationLock = false;
            mapControl.Refresh();
        }
    }
}