using ACadSharp;
using ACadSharp.Entities;
using ACadSharp.IO;
using CSMath;
using System.Text.RegularExpressions;

namespace TestCAD
{
    internal class CadDocReader
    {
        public CadDocReader(string filePath)
        {
            this.FilePath = filePath;
        }

        public string FilePath { get; private set; }

        public void ReadTable()
        {
            string filePath = this.FilePath;
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }
            CadDocument cadDocument = LoadCadDocument(filePath);
            foreach (var blockRecord in cadDocument.BlockRecords)
            {
                if (!blockRecord.Name.StartsWith("*T"))
                {
                    continue;
                }
                if (blockRecord.Entities.Count < 1)
                {
                    continue;
                }
                for (int i = 0; i < blockRecord.Entities.Count; ++i)
                {
                    Entity entity = blockRecord.Entities[i];
                    if (entity.ObjectName.EndsWith("TEXT"))
                    {
                        MText mText = entity as MText;
                        string entityText;
                        XYZ insertPoint;
                        if (mText == null)
                        {
                            TextEntity textEntity = entity as TextEntity;
                            entityText = textEntity.Value;
                            insertPoint = textEntity.InsertPoint;
                        }
                        else
                        {
                            entityText = mText.Value;
                            insertPoint = mText.InsertPoint;
                        }
                        if (!string.IsNullOrEmpty(entityText))
                        {
                            string pattern = @"\\f.*?;";
                            entityText = Regex.Replace(entityText, pattern, string.Empty).TrimStart('{').TrimEnd('}');
                        }
                        Console.Write(entityText);
                        Console.Write("\t");
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 加载CAD文档
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>CAD文档</returns>
        /// <exception cref="NotSupportedException">文件类型不支持异常</exception>
        private CadDocument LoadCadDocument(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            CadDocument cadDocument = null;
            if (extension.Equals(".dwg", StringComparison.OrdinalIgnoreCase))
            {
                DwgReader dwgReader = null;
                try
                {
                    dwgReader = new DwgReader(filePath);
                    cadDocument = dwgReader.Read();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    dwgReader?.Dispose();
                }
            }
            else if (extension.Equals(".dxf", StringComparison.OrdinalIgnoreCase))
            {
                DxfReader dxfReader = null;
                try
                {
                    dxfReader = new DxfReader(filePath);
                    cadDocument = dxfReader.Read();
                }
                catch
                {
                    throw;
                }
                finally
                {
                    dxfReader?.Dispose();
                }
            }
            else
            {
                throw new NotSupportedException($"文件：{filePath}，不支持类型：{extension}");
            }
            return cadDocument;
        }
    }
}
