using ArcGIS.Core.Geometry;
using ArcGIS.Core.Hosting;
using ArcGIS.Core.Internal.Geometry;
using Assimp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace TestColladaToObjProCoreHost
{
    internal class Program
    {
        //[STAThread] must be present on the Application entry point
        [STAThread]
        static void Main(string[] args)
        {
            //Call Host.Initialize before constructing any objects from ArcGIS.Core
            Host.Initialize();
            //TODO: Add your business logic here.   

            string testDir = Path.Combine(AppContext.BaseDirectory, "test\\");
            string outputDir = Path.Combine(testDir, "obj(kml)");
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            var daeFiles = Directory.GetFiles(testDir, "*.dae");
            foreach (var daeFile in daeFiles)
            {
                string kmlFile = Path.ChangeExtension(daeFile, ".kml");
                if (!File.Exists(kmlFile))
                {
                    Console.WriteLine($"KML file not found for {daeFile}. Skipping.");
                    continue;
                }
                string outputFile = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(daeFile) + ".obj");
                ConvertColladaKmlToObj(daeFile, kmlFile, outputFile);
            }
        }

        static void ConvertColladaKmlToObj(string daeFilePath, string kmlFilePath, string objOutputPath)
        {
            // Step 1: Parse the .kml file
            var modelInfo = ParseKmlFile(kmlFilePath);
            if (modelInfo == null)
            {
                Console.WriteLine("Failed to parse KML file.");
                return;
            }

            Console.WriteLine($"Parsed KML Data: Longitude={modelInfo.Longitude}, Latitude={modelInfo.Latitude}, Altitude={modelInfo.Altitude}");

            // Step 2: Load .dae file using Assimp
            AssimpContext context = new AssimpContext();
            Scene scene = context.ImportFile(daeFilePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateNormals);

            // Step 3: Adjust vertices based on KML location
            foreach (var mesh in scene.Meshes)
            {
                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    mesh.Vertices[i] = AdjustVertex(mesh.Vertices[i], modelInfo.Longitude, modelInfo.Latitude, modelInfo.Altitude);
                }
            }

            // Step 4: Export the modified scene to .obj
            context.ExportFile(scene, objOutputPath, "obj");
            Console.WriteLine($"Converted file saved to {objOutputPath}");
        }

        static ModelInfo? ParseKmlFile(string kmlFilePath)
        {
            if (!File.Exists(kmlFilePath))
            {
                Console.WriteLine("KML file does not exist.");
                return null;
            }

            try
            {
                XDocument kmlDoc = XDocument.Load(kmlFilePath);
                XNamespace ns = "http://www.opengis.net/kml/2.2";

                var location = kmlDoc.Descendants(ns + "Location").FirstOrDefault();
                if (location != null)
                {
                    double longitude = double.Parse(location.Element(ns + "longitude")?.Value ?? "0");
                    double latitude = double.Parse(location.Element(ns + "latitude")?.Value ?? "0");
                    double altitude = double.Parse(location.Element(ns + "altitude")?.Value ?? "0");

                    return new ModelInfo
                    {
                        Longitude = longitude,
                        Latitude = latitude,
                        Altitude = altitude
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing KML file: {ex.Message}");
            }

            return null;
        }

        static Vector3D AdjustVertex(Vector3D vertex, double longitude, double latitude, double altitude)
        {
            // Example adjustment logic: Translate the vertex by the geolocation values
            (double lon_merc, double lat_merc) = WGS84ToWebMercator(longitude, latitude);
            vertex.X += (float)(lon_merc);
            vertex.Y += (float)(lat_merc);
            vertex.Z += (float)(altitude);
            return vertex;
        }

        static (double, double) WGS84ToWebMercator(double longitude, double latitude)
        {
            MapPoint origin = MapPointBuilder.CreateMapPoint(longitude, latitude, SpatialReferences.WGS84);
            SpatialReference wgs84_webmercator = SpatialReferenceBuilder.CreateSpatialReference(3857);
            MapPoint projectedPoint = GeometryEngine.Instance.Project(origin, wgs84_webmercator) as MapPoint;

            return (projectedPoint.X, projectedPoint.Y);
        }

        class ModelInfo
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public double Altitude { get; set; }
        }
    }
}
