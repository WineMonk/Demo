using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConfigurableOptions.Settings;

namespace TestConfigurableOptions.Validators
{
    public class TestSettingsValidator : IValidateOptions<TestSettings>
    {
        public ValidateOptionsResult Validate(string? name, TestSettings options)
        {
            if (string.IsNullOrEmpty(options.StringSetting))
            {
                return ValidateOptionsResult.Fail("StringSetting cannot be null or empty.");
            }
            if (options.DoubleSetting >= 1)
            {
                return ValidateOptionsResult.Fail("DoubleSetting must be greater than 1.");
            }
            return ValidateOptionsResult.Success;
        }
    }
}
