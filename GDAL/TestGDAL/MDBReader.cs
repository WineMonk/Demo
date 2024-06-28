using OSGeo.OGR;
using System.Text;

namespace TestGDAL
{
    public class MDBReader
    {
        public void Read(string mdbFilePath)
        {
            // 注册所有驱动
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();


            // 注册编码提供程序以支持GBK编码
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // 打开MDB数据源
            var mdbDriver = Ogr.GetDriverByName("PGeo");
            DataSource mdbDataSource = mdbDriver.Open(mdbFilePath, 0);
            if (mdbDataSource == null)
            {
                Console.WriteLine("Failed to open MDB file.");
                return;
            }

            // 遍历MDB数据源中的所有图层并复制到GDB数据源
            for (int i = 0; i < mdbDataSource.GetLayerCount(); i++)
            {
                Layer mdbLayer = mdbDataSource.GetLayerByIndex(i);
                string lyrName = mdbLayer.GetName();
                WriteTitle($"Layer: {lyrName}");
                List<FieldDefn> fieldDefns = new List<FieldDefn>();
                FeatureDefn mdbFeatureDefn = mdbLayer.GetLayerDefn();
                for (int j = 0; j < mdbFeatureDefn.GetFieldCount(); j++)
                {
                    FieldDefn fieldDefn = mdbFeatureDefn.GetFieldDefn(j);
                    fieldDefns.Add(fieldDefn);
                }
                Console.WriteLine(string.Join(',', fieldDefns.Select(fd => fd.GetName())));
                mdbLayer.ResetReading();
                Feature feature;
                while ((feature = mdbLayer.GetNextFeature()) != null)
                {
                    try
                    {
                        List<string> values = new List<string>();
                        for (int j = 0; j < fieldDefns.Count; j++)
                        {
                            FieldDefn fieldDefn = fieldDefns[j];
                            string fieldName = fieldDefn.GetName();
                            FieldType fieldType = fieldDefn.GetFieldType();
                            switch (fieldType)
                            {
                                case FieldType.OFTInteger:
                                    int valInt = feature.GetFieldAsInteger(fieldName);
                                    values.Add(valInt + "");
                                    break;
                                case FieldType.OFTString:
                                    byte[] valStr = feature.GetFieldAsBinary(fieldName, FeatureDatastoreType.Other);
                                    string gbkStr = Encoding.GetEncoding("GBK").GetString(valStr);
                                    values.Add(gbkStr);
                                    break;
                                case FieldType.OFTWideString:
                                    byte[] valWideStr = feature.GetFieldAsBinary(fieldName, FeatureDatastoreType.Other);
                                    string gbkWideStr = Encoding.GetEncoding("GBK").GetString(valWideStr);
                                    values.Add(gbkWideStr);
                                    break;
                                case FieldType.OFTDate:
                                    feature.GetFieldAsDateTime(
                                        fieldName,
                                        out int dyear,
                                        out int dmonth,
                                        out int dday,
                                        out int dhour,
                                        out int dminute,
                                        out float dsecond,
                                        out int dtzFlag);
                                    DateTime valDate = new DateTime(dyear, dmonth, dday, dhour, dminute, Convert.ToInt32(dsecond));
                                    values.Add(valDate.ToString());
                                    break;
                                case FieldType.OFTTime:
                                    feature.GetFieldAsDateTime(
                                        fieldName,
                                        out int tyear,
                                        out int tmonth,
                                        out int tday,
                                        out int thour,
                                        out int tminute,
                                        out float tsecond,
                                        out int ttzFlag);
                                    DateTime valTime = new DateTime(tyear, tmonth, tday, thour, tminute, Convert.ToInt32(tsecond));
                                    values.Add(valTime.ToString());
                                    break;
                                case FieldType.OFTDateTime:
                                    feature.GetFieldAsDateTime(
                                        fieldName,
                                        out int dtyear,
                                        out int dtmonth,
                                        out int dtday,
                                        out int dthour,
                                        out int dtminute,
                                        out float dtsecond,
                                        out int dttzFlag);
                                    DateTime valDateTime = new DateTime(dtyear, dtmonth, dtday, dthour, dtminute, Convert.ToInt32(dtsecond));
                                    values.Add(valDateTime.ToString());
                                    break;
                                case FieldType.OFTInteger64:
                                    long valLon = feature.GetFieldAsInteger64(fieldName);
                                    values.Add(valLon + "");
                                    break;
                                case FieldType.OFTReal:
                                    double valReal = feature.GetFieldAsDouble(fieldName);
                                    if (valReal == null)
                                    {
                                        values.Add(string.Empty);
                                    }
                                    else
                                    {
                                        values.Add(valReal.ToString());
                                    }
                                    break;
                                default:
                                    values.Add(string.Empty);
                                    break;
                            }
                        }
                        Console.WriteLine(string.Join(",", values));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    finally
                    {
                        feature.Dispose();
                    }
                }
                Console.WriteLine();
            }
            // 释放资源
            mdbDataSource.Dispose();
        }

        public void WriteTitle(string title)
        {
            if (title == null)
            {
                return;
            }
            int length = title.Length;
            if (length == 0)
            {
                return;
            }
            int starLength = (length / 4 + 1) * 6;
            Console.WriteLine(new string('*', starLength));
            Console.WriteLine($"*{new string(' ', starLength - 2)}*");
            Console.WriteLine($"* {title}{new string(' ', starLength - length - 4)} *");
            Console.WriteLine($"*{new string(' ', starLength - 2)}*");
            Console.WriteLine(new string('*', starLength));
            Console.WriteLine();
        }
    }
}
