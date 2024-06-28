using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using System.Text;

namespace TestGDAL
{
    public class GDBReader
    {
        public void Read(string mdbFilePath)
        {
            string strDataPath = @".\Data\Default.gdb";

            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();

            Gdal.SetConfigOption("SHAPE_ENCODING", "UTF-8");

            // 打开 FileGDB
            using (var driver = Ogr.GetDriverByName("OpenFileGDB"))
            using (var dataSource = driver.Open(strDataPath, 0))
            {
                if (dataSource == null)
                {
                    Console.WriteLine("Failed to open FileGDB.");
                    return;
                }

                List<Layer> layers = new List<Layer>();
                int layerCount = dataSource.GetLayerCount();
                for (int i = 0; i < layerCount; i++)
                {
                    Layer layer = dataSource.GetLayerByIndex(i);
                    layers.Add(layer);
                }

                Console.WriteLine("GDB Layers: ");
                foreach (var layer in layers)
                {
                    string layerName = layer.GetName();
                    wkbGeometryType wkbGeometryType = layer.GetGeomType();
                    SpatialReference spatialReference = layer.GetSpatialRef();
                    Console.WriteLine($"LayerName: {layerName}\tGeometryType: {wkbGeometryType}\tSpatialReference: {spatialReference.GetName()}");
                }
                Console.WriteLine();

                Console.WriteLine("Geometry: ");
                foreach (var layer in layers)
                {
                    Console.WriteLine(layer.GetName());
                    Console.WriteLine("Geometry Wkt");

                    layer.ResetReading();
                    Feature feature;
                    while ((feature = layer.GetNextFeature()) != null)
                    {
                        try
                        {
                            Geometry geometry = feature.GetGeometryRef();
                            int v = geometry.ExportToWkt(out string wkt);
                            Console.WriteLine(wkt);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            feature.Dispose();
                        }
                    }
                }
                Console.WriteLine();

                Console.WriteLine("Attribute Table: ");
                foreach (var layer in layers)
                {
                    Console.WriteLine(layer.GetName());
                    FeatureDefn featureDefn = layer.GetLayerDefn();
                    List<FieldDefn> fieldDefns = new List<FieldDefn>();
                    int fieldCount = featureDefn.GetFieldCount();
                    for (int i = 0; i < fieldCount; i++)
                    {
                        FieldDefn fieldDefn = featureDefn.GetFieldDefn(i);
                        fieldDefns.Add(fieldDefn);
                    }
                    Console.WriteLine(string.Join(",", fieldDefns.Select(f => f.GetName())));

                    layer.ResetReading();
                    Feature feature;
                    while ((feature = layer.GetNextFeature()) != null)
                    {
                        try
                        {
                            List<string> values = new List<string>();
                            for (int i = 0; i < fieldDefns.Count; i++)
                            {
                                FieldDefn fieldDefn = fieldDefns[i];
                                string fieldName = fieldDefn.GetName();
                                FieldType fieldType = fieldDefn.GetFieldType();
                                switch (fieldType)
                                {
                                    case FieldType.OFTInteger:
                                        int valInt = feature.GetFieldAsInteger(fieldName);
                                        values.Add(valInt + "");
                                        break;
                                    case FieldType.OFTString:
                                        string valStr = feature.GetFieldAsString(fieldName);
                                        string utf8Str = Encoding.UTF8.GetString(Encoding.Default.GetBytes(valStr));
                                        values.Add(utf8Str);
                                        break;
                                    case FieldType.OFTWideString:
                                        string valWidStr = feature.GetFieldAsString(fieldName);
                                        string utf8WidStr = Encoding.UTF8.GetString(Encoding.Default.GetBytes(valWidStr));
                                        values.Add(utf8WidStr);
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
                                    default:
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
                }
            }
        }

    }
}
