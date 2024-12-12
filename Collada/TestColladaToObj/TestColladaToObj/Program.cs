using Assimp;
using System.Xml.Linq;

namespace TestColladaToObj
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //// 输入的 .dae 文件列表
            //string testDir = Path.Combine(AppContext.BaseDirectory, "test\\");
            //var inputFiles = Directory.GetFiles(testDir, "*.dae");
            //// 输出的 .obj 文件路径
            //var outputFile = Path.Combine(testDir, "merged.obj");
            //// 合并 .dae 文件
            //MergeColladaToObj(inputFiles, outputFile);

            //string testDir = Path.Combine(AppContext.BaseDirectory, "test\\");
            //var inputFiles = Directory.GetFiles(testDir, "*.dae");
            //foreach (var file in inputFiles)
            //{
            //    string outputDir = Path.Combine(testDir, "obj");
            //    if (!Directory.Exists(outputDir))
            //    {
            //        Directory.CreateDirectory(outputDir);
            //    }
            //    string outputFilePath = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(file) + ".obj");
            //    ConvertColladaToObj(file, outputFilePath);
            //}
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

        static void MergeColladaToObj(string[] inputFiles, string outputFile)
        {
            // 创建 Assimp 上下文
            var assimpContext = new AssimpContext();

            // 合并场景
            Console.WriteLine("开始合并场景...");
            var mergedScene = new Scene();

            // 合并根节点
            mergedScene.RootNode = new Node("Root");

            foreach (var file in inputFiles)
            {
                Console.WriteLine($"加载文件: {file}");
                var scene = assimpContext.ImportFile(file, PostProcessSteps.Triangulate);

                if (scene == null)
                {
                    Console.WriteLine($"无法加载文件: {file}");
                    continue;
                }

                // 合并节点
                foreach (var mesh in scene.Meshes)
                {
                    mergedScene.Meshes.Add(mesh);
                }

                foreach (var material in scene.Materials)
                {
                    mergedScene.Materials.Add(material);
                }

                // 将当前文件的根节点添加到合并场景的根节点
                foreach (var childNode in scene.RootNode.Children)
                {
                    mergedScene.RootNode.Children.Add(childNode);
                }
            }

            // 将合并后的场景导出为 .obj 文件
            Console.WriteLine($"导出到 .obj 文件: {outputFile}");
            assimpContext.ExportFile(mergedScene, outputFile, "obj");

            Console.WriteLine("转换完成！");
        }

        static void ConvertColladaToObj(string daeFilePath, string objFilePath)
        {
            // 创建 Assimp 导入/导出对象
            AssimpContext context = new AssimpContext();
            //Scene scene = context.ImportFile(objFilePath, PostProcessSteps.Triangulate);
            //context.ExportFile(scene, daeFilePath, "collada");
            Scene scene = context.ImportFile(daeFilePath, PostProcessSteps.Triangulate);
            context.ExportFile(scene, objFilePath, "obj");
            Console.WriteLine($"{daeFilePath} 成功转换为 {objFilePath}");
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
            const double R = 6378137; // Web Mercator 地球半径（单位：米）

            // 将经纬度转换为弧度
            double lonRad = longitude * Math.PI / 180.0;
            double latRad = latitude * Math.PI / 180.0;

            // 计算 X 和 Y
            double x = R * lonRad;
            double y = R * Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0));

            return (x, y);
        }

        class ModelInfo
        {
            public double Longitude { get; set; }
            public double Latitude { get; set; }
            public double Altitude { get; set; }
        }
    }
}
