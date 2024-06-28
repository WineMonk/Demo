using System.ComponentModel;
using System.Windows;
using ViewModelLocator.Views.v1;

namespace ViewModelLocator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IContainerExtension _container;
        public MainWindow(IContainerExtension container)
        {
            InitializeComponent();
            _container = container;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _container.Resolve<TestWindow>().ShowDialog();
        }
    }
}
