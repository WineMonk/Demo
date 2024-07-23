using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Data;
using System.Runtime;
using System.Windows;
using TestConfigurableOptions.Configurators;
using TestConfigurableOptions.Settings;
using TestConfigurableOptions.Validators;

namespace TestConfigurableOptions
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // services.AddSingleton<IConfigureOptions<TestSettings>, TestConfigureOptionsSettingsConfigurator>();
                    services.Configure<TestSettings>(context.Configuration.GetSection("TestSettings"));
                    services.AddSingleton<IValidateOptions<TestSettings>, TestSettingsValidator>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();
            _host.Start();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host?.Dispose();
            base.OnExit(e);
        }
    }
}
