using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfigurableOptions.Settings
{
    public class TestSettings : SettingsBase
    {
        public string StringSetting { get; set; }
        public int IntSetting { get; set; }
        public bool BoolSetting { get; set; }
        public double DoubleSetting { get; set; }
        public DateTime DateTimeSetting { get; set; }
        public List<string> ListSetting { get; set; }
    }
}
