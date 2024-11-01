using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestCommunityToolkit._3_Command;
using TestCommunityToolkit._4_IoC.ViewModel;

namespace TestCommunityToolkit
{
    /// <summary>
    /// App.xaml的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BlogWindow window = new BlogWindow();
            window.ShowDialog();
        }

        public App() : base()
        {
            Services = ConfigureServices();
        }

        /// <summary>
        /// 获取当前正在使用的 <see cref="App"/> 实例。
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// 获取 <see cref="IServiceProvider"/> 实例以解析应用程序服务。
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// 配置应用程序的服务。
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<PostWindowViewModel>();
            return services.BuildServiceProvider();
        }
    }

}
