using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfigurableOptions.Settings
{
    public class SettingsBase
    {
        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.GetType().GetProperties().Select(p =>
                {
                    if (p.GetValue(this, null) is IList list)
                    {
                        return p.Name + " = " + string.Join(", ", list.Cast<object>());
                    }
                    return p.Name + " = " + p.GetValue(this, null);
                }));
        }
    }
}
