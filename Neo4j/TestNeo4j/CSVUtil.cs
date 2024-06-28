using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestNeo4j
{
    internal class CSVUtil
    {
        /// <summary>
        /// CSV 转 DataTable
        /// </summary>
        /// <param name="csvFilePath">CSV文件路径</param>
        /// <param name="separator">间隔符，默认为","</param>
        /// <returns>结果DataTable</returns>
        public static DataTable? FromCSV(string csvFilePath, string separator = ",")
        {
            DataTable dt = new DataTable();
            string separatorTemp = Guid.NewGuid().ToString().Replace("-", "");
            List<string[]?> lineArrayList = ReadCsvLines(csvFilePath, separator, separatorTemp);
            if (lineArrayList.Count < 1)
            {
                return null;
            }

            int maxColumnCount = 0;
            lineArrayList.ForEach(l =>
            {
                if (l != null && l.Length > maxColumnCount)
                {
                    maxColumnCount = l.Length;
                }
            });

            string[]? headerLineArray = lineArrayList[0];
            for (int columnIdx = 0; columnIdx < maxColumnCount; columnIdx++)
            {
                string? columnName = null;
                if (headerLineArray != null && headerLineArray.Length > columnIdx)
                {
                    columnName = headerLineArray[columnIdx]?.Trim('"')?.Replace(separatorTemp, separator);
                }
                if (string.IsNullOrEmpty(columnName))
                {
                    columnName = $"column_{columnIdx + 1}";
                }
                string columnNameTemp = columnName;
                int tag = 0;
                while (dt.Columns.Contains(columnNameTemp))
                {
                    columnNameTemp = $"{columnName}_{++tag}";
                }
                dt.Columns.Add(columnNameTemp);
            }
            for (int rowIdx = 1; rowIdx < lineArrayList.Count; rowIdx++)
            {
                string[]? lineArray = lineArrayList[rowIdx];
                DataRow dataRow = dt.NewRow();
                for (int columnIdx = 0; columnIdx < maxColumnCount; columnIdx++)
                {
                    if (lineArray != null && lineArray.Length > columnIdx)
                    {
                        dataRow[columnIdx] = lineArray[columnIdx]?.Trim('\"')?.Replace(separatorTemp, separator);
                    }
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        private static List<string[]?> ReadCsvLines(string csvFilePath, string separator, string separatorTemp)
        {
            FileStream? fs = null;
            StreamReader? sr = null;
            List<string[]?> lineArrayList = new List<string[]?>();
            try
            {
                fs = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, Encoding.UTF8);
                string? currentLine = string.Empty;
                string[]? lineArray = null;
                while (!string.IsNullOrEmpty(currentLine = sr.ReadLine()))
                {
                    currentLine = currentLine.Trim();
                    if (currentLine.Contains('"'))
                    {
                        Regex regex = new Regex("\"(.*?)\"");
                        MatchCollection matches = regex.Matches(currentLine);
                        int offset = 0;
                        foreach (Match match in matches.Cast<Match>())
                        {
                            Group group = match.Groups[1];
                            if (group.Value.Contains(separator))
                            {
                                string replaceText = group.Value.Replace(separator, separatorTemp);
                                currentLine = currentLine.Remove(group.Index + offset, group.Length);
                                currentLine = currentLine.Insert(group.Index + offset, replaceText);
                                offset = offset + replaceText.Length - group.Length;
                            }
                        }
                    }
                    lineArray = currentLine.Split(separator);
                    lineArrayList.Add(lineArray);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                try { sr?.Close(); } catch { }
                try { fs?.Close(); } catch { }
            }
            return lineArrayList;
        }
    }
}
