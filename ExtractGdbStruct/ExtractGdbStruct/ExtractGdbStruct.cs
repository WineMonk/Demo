using ArcGIS.Core.Data;
using ArcGIS.Core.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace ExtractGdbStruct
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Host.Initialize();
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string jsonPath = Path.Combine(baseDirectory, "gdb_list.json");
            string jsonText = File.ReadAllText(jsonPath);
            JsonNode jsonNode = JsonNode.Parse(jsonText)["gdb_list"];
            List<string> gdbList = jsonNode.Deserialize<List<string>>();
            if (gdbList == null || gdbList.Count < 1)
            {
                Console.WriteLine("请指定要提取的gdb路径！");
                return;
            }
            foreach (var gdbPath in gdbList)
            {
                Console.WriteLine($"正在提取 - {gdbPath}...");
                if (string.IsNullOrEmpty(gdbPath) || !Directory.Exists(gdbPath))
                {
                    Console.WriteLine("gdb不存在！");
                    continue;
                }
                try
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("tableName");
                    dataTable.Columns.Add("fieldName");
                    dataTable.Columns.Add("aliasName");
                    dataTable.Columns.Add("fieldType");
                    dataTable.Columns.Add("nullable");
                    dataTable.Columns.Add("defaultValue");
                    dataTable.Columns.Add("precision");
                    dataTable.Columns.Add("scale");
                    dataTable.Columns.Add("length");
                    dataTable.TableName = Path.GetFileNameWithoutExtension(gdbPath);
                    Uri uri = new Uri(gdbPath);
                    FileGeodatabaseConnectionPath fgcp = new FileGeodatabaseConnectionPath(uri);
                    using (Geodatabase gdb = new Geodatabase(fgcp))
                    {
                        IReadOnlyList<FeatureClassDefinition> definitions = gdb.GetDefinitions<FeatureClassDefinition>();
                        foreach (var defi in definitions)
                        {
                            foreach (var fld in defi.GetFields())
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[]
                                {
                                    defi.GetName(),
                                    fld.Name,
                                    fld.AliasName,
                                    fld.FieldType,
                                    fld.IsNullable,
                                    fld.GetDefaultValue(),
                                    fld.Precision,
                                    fld.Scale,
                                    fld.Length
                                };
                                dataTable.Rows.Add(dataRow);
                            }
                        }
                    }
                    Export(dataTable);
                    Console.WriteLine("提取完成！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("提取失败：" + ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }
        static void Export(DataTable dt)
        {
            string fp = string.Empty;
            fp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "out");
            if (!Directory.Exists(fp))
            {
                Directory.CreateDirectory(fp);
            }
            fp = Path.Combine(fp, dt.TableName + ".csv");
            List<string> lines = new List<string>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                lines.Add(dt.Columns[i].ColumnName);
            }
            string header = string.Join(',', lines);
            lines.Clear();
            lines.Add(header);
            foreach (DataRow row in dt.Rows)
            {
                lines.Add(string.Join(',', row.ItemArray));
            }
            File.AppendAllLines(Guid.NewGuid() + ".csv", lines);
        }
    }
}
