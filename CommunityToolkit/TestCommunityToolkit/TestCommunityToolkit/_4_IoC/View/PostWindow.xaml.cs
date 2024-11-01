using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestCommunityToolkit._4_IoC.ViewModel;

namespace TestCommunityToolkit._4_IoC.View
{
    /// <summary>
    /// PostWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PostWindow : Window
    {
        public PostWindow()
        {
            InitializeComponent();
            this.DataContext = App.Current.Services.GetService<PostWindowViewModel>();
        }
    }
}
