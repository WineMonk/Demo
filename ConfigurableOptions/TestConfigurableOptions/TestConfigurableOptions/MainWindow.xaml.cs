using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Runtime;
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
using TestConfigurableOptions.Settings;

namespace TestConfigurableOptions
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IOptionsMonitor<TestSettings> _testOptionsMonitor;
        private readonly IOptions<TestSettings> _testConfigureOptions;
        private readonly IValidateOptions<TestSettings> _testValidateOptions;
        public MainWindow(IOptions<TestSettings> testConfigureOptions, IOptionsMonitor<TestSettings> testOptionsMonitor, IValidateOptions<TestSettings> testValidateOptions)
        {
            InitializeComponent();
            _testOptionsMonitor = testOptionsMonitor;
            _testConfigureOptions = testConfigureOptions;
            _testValidateOptions = testValidateOptions;

            // 注册配置变化监听器
            _testOptionsMonitor.OnChange(settings =>
            {
                var result = _testValidateOptions.Validate(null, settings);
                if (!result.Succeeded)
                {
                    foreach (var failure in result.Failures)
                    {
                        MessageBox.Show($"Validation failed: {failure}");
                    }
                }
                else
                {
                    ShowSettings(settings);
                }
            });
        }

        private void ShowSettings(SettingsBase settings)
        {
            MessageBox.Show(settings.ToString(), settings.GetType().Name);
        }

        private void buttonTestOptionsMonitorSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = _testValidateOptions.Validate(null, _testOptionsMonitor.CurrentValue);
                if (!result.Succeeded)
                {
                    foreach (var failure in result.Failures)
                    {
                        MessageBox.Show($"Validation failed: {failure}");
                    }
                }
                ShowSettings(_testOptionsMonitor.CurrentValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        private void buttonTestConfigureOptions_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings(_testConfigureOptions.Value);
        }
    }
}