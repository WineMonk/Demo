using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGDAL
{
    public class MDB2GDB
    {
        public void Convert(string mdbFilePath, string gdbFilePath)
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

            // 创建GDB数据源
            Driver gdbDriver = Ogr.GetDriverByName("OpenFileGDB");
            if (gdbDriver == null)
            {
                Console.WriteLine("FileGDB driver is not available.");
                return;
            }

            DataSource gdbDataSource = gdbDriver.CreateDataSource(gdbFilePath, null);
            if (gdbDataSource == null)
            {
                Console.WriteLine("Failed to create GDB file.");
                return;
            }

            // 遍历MDB数据源中的所有图层并复制到GDB数据源
            for (int i = 0; i < mdbDataSource.GetLayerCount(); i++)
            {
                Layer mdbLayer = mdbDataSource.GetLayerByIndex(i);
                string lyrName = mdbLayer.GetName();
                Layer gdbLayer = gdbDataSource.CreateLayer(lyrName, mdbLayer.GetSpatialRef(), mdbLayer.GetGeomType(), null);
                Console.WriteLine($"Source Layer: {lyrName}");
                // 复制字段定义
                FeatureDefn mdbFeatureDefn = mdbLayer.GetLayerDefn();
                for (int j = 0; j < mdbFeatureDefn.GetFieldCount(); j++)
                {
                    FieldDefn fieldDefn = mdbFeatureDefn.GetFieldDefn(j);
                    string fieldName = fieldDefn.GetName();

                    Console.WriteLine($"Source Field: {fieldDefn.GetName()} - Right Field: {fieldName}");
                    gdbLayer.CreateField(fieldDefn, 1);
                }

                mdbLayer.ResetReading();
                // 复制要素
                Feature mdbFeature;
                while ((mdbFeature = mdbLayer.GetNextFeature()) != null)
                {
                    Feature gdbFeature = new Feature(gdbLayer.GetLayerDefn());
                    gdbFeature.SetFrom(mdbFeature, 1);

                    // 显式设置字段的字符编码
                    for (int j = 0; j < gdbFeature.GetFieldCount(); j++)
                    {
                        FieldType fieldType = gdbFeature.GetFieldDefnRef(j).GetFieldType();
                        if (fieldType == FieldType.OFTString || fieldType == FieldType.OFTWideString)
                        {
                            byte[] bytes = gdbFeature.GetFieldAsBinary(j, FeatureDatastoreType.Other);
                            string fieldValue = Encoding.GetEncoding("GBK").GetString(bytes);
                            gdbFeature.SetField(j, fieldValue);
                        }
                    }

                    gdbLayer.CreateFeature(gdbFeature);
                    mdbFeature.Dispose();
                    gdbFeature.Dispose();
                }
            }

            // 释放资源
            mdbDataSource.Dispose();
            gdbDataSource.Dispose();

            Console.WriteLine("Conversion completed successfully.");
        }
    }
}
