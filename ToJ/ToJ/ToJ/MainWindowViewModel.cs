using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ToJ
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private string _codeType = "C#";

        [ObservableProperty]
        private string _codeText;
        [ObservableProperty]
        private string _jsonText;

        [RelayCommand]
        private void SelectCodeType(string codeType)
        {
            _codeType = codeType;
        }

        [RelayCommand]
        private void ToJson()
        {
            if (string.IsNullOrEmpty(CodeText))
            {
                return;
            }
            if (_codeType == "C#")
            {
                // 解析C#代码文件
                var tree = CSharpSyntaxTree.ParseText(CodeText);
                var root = tree.GetRoot();

                // 查找类声明
                var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                if (classNode == null)
                {
                    Console.WriteLine("No class found in the provided C# file.");
                    return;
                }

                // 查找属性声明并提取默认值
                var settings = classNode.Members
                    .OfType<PropertyDeclarationSyntax>()
                    .Select(prop => new
                    {
                        Name = prop.Identifier.Text,
                        Type = prop.Type.ToString(),
                        DefaultValue = prop.Initializer?.Value.ToString().Trim('"')
                    })
                    .ToDictionary(p => p.Name, p => ConvertValue(p.Type, p.DefaultValue));

                // 序列化为JSON
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                JsonText = JsonSerializer.Serialize(new { MySettings = settings }, options);
            }
            else if (_codeType == "Java")
            {

            }
            else if (_codeType == "Python")
            {

            }
        }
        //[RelayCommand(CanExecute = nameof(CanToCode))]
        [RelayCommand]
        private void ToCode()
        {
            if (string.IsNullOrEmpty(JsonText))
            {
                return;
            }
            if (_codeType == "C#")
            {
                // 反序列化JSON为对象
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(JsonText);

                if (jsonObject == null)
                {
                    Console.WriteLine("Failed to deserialize JSON.");
                    return;
                }
                string key = jsonObject.First().Key;
                Dictionary<string, object> value = jsonObject.First().Value;
                // 生成C#类代码
                CodeText = GenerateCSharpClass(key, value);
            }
        }

        static string GenerateCSharpClass(string className, Dictionary<string, object> settings)
        {
            string newLine = Environment.NewLine;
            string tab = "    ";
            // 构建C#类代码
            string code = $@"/// <summary>{newLine}/// {newLine}/// </summary>{newLine}public class {className}{newLine}{{{newLine}";
            foreach (var setting in settings)
            {
                string settingName = setting.Key;
                object settingValue = setting.Value;
                string settingType = GetCSharpType(settingValue);

                // 处理字符串类型的值需要进行双引号的转义

                code += $@"{tab}/// <summary>{newLine}{tab}/// {newLine}{tab}/// </summary>{newLine}";
                code += $@"{tab}public {settingType} {settingName} {{ get; set; }}";
                if (settingValue != null)
                {
                    string settingValueStr = settingType == "string" ? $"\"{EscapeDoubleQuotes(settingValue.ToString())}\"" : settingValue.ToString();
                    code += $@" = {settingValueStr};";
                }
                code += newLine;
            }
            code += "}";
            return code;
        }

        static string GetCSharpType(object value)
        {
            if (value == null || !(value is JsonElement jsonElement))
            {
                return "string";
            }
            JsonValueKind valueKind = jsonElement.ValueKind;
            switch (valueKind)
            {
                case JsonValueKind.Undefined:
                    return "string";
                case JsonValueKind.Object:
                    return "object";
                case JsonValueKind.Array:
                    return "List<string>";
                case JsonValueKind.String:
                    return "string";
                case JsonValueKind.Number:
                    return "int";
                case JsonValueKind.True:
                    return "bool";
                case JsonValueKind.False:
                    return "bool";
                case JsonValueKind.Null:
                    return "string";
                default:
                    return "string";
            }
        }

        static string EscapeDoubleQuotes(string input)
        {
            // 在C#字符串中转义双引号
            return input.Replace("\"", "\\\"");
        }

        static object ConvertValue(string type, string value)
        {
            return type switch
            {
                "string" => value,
                "int" => int.TryParse(value, out var i) ? i : 0,
                _ => value
            };
        }
    }
}
