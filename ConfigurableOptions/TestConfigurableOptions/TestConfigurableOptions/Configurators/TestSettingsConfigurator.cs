using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConfigurableOptions.Settings;

namespace TestConfigurableOptions.Configurators
{
    public class TestSettingsConfigurator : IConfigureOptions<TestSettings>
    {
        public void Configure(TestSettings options)
        {
            options.BoolSetting = true;
            options.DateTimeSetting = DateTime.Now;
            options.DoubleSetting = 0.123456789;
            options.IntSetting = 123456789;
            options.ListSetting = new List<string> { "Item1", "Item2", "Item3" };
            options.StringSetting = "Test String";
        }
    }
}
