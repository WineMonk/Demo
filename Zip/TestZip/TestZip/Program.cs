using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.Intrinsics.Arm;

namespace TestZip
{
    internal class Program
    {
        static void Main()
        {
            string sourceDirectory = AppDomain.CurrentDomain.BaseDirectory + "gdal";
            string destinationZipFilePath = AppDomain.CurrentDomain.BaseDirectory + "gdal.zip";


            System.IO.Compression.ZipFile.CreateFromDirectory(sourceDirectory, destinationZipFilePath, CompressionLevel.Optimal, false);

            //// 压缩文件夹
            //CompressFolder(sourceDirectory, destinationZipFilePath);

            Console.WriteLine("文件夹已成功压缩到 " + destinationZipFilePath);
        }

        static void CompressFolder(string sourceDir, string zipFilePath)
        {
            // 创建目标zip文件
            using (FileStream fsOut = File.Create(zipFilePath))
            using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
            {
                zipStream.SetLevel(3); // 设置压缩级别（0-9）

                // 遍历源文件夹中的所有文件和目录
                int folderOffset = sourceDir.Length + (sourceDir.EndsWith("\\") ? 0 : 1);

                CompressDirectory(sourceDir, zipStream, folderOffset);
            }
        }

        static void CompressDirectory(string path, ZipOutputStream zipStream, int folderOffset)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {
                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // 创建条目名
                entryName = ZipEntry.CleanName(entryName); // 清理名称

                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // 设置条目的日期时间
                newEntry.Size = fi.Length; // 设置条目的大小

                zipStream.PutNextEntry(newEntry);

                // 写入文件内容
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }

            // 递归处理目录
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressDirectory(folder, zipStream, folderOffset);
            }
        }

        //static void Main(string[] args)
        //{
        //    // 9 - 137136 - 351221
        //    // 5 - 35895 - 354376
        //    // 1 - 24233 - 360197
        //    //string rootDir = AppDomain.CurrentDomain.BaseDirectory + "GISAPP";
        //    //string zipFile = AppDomain.CurrentDomain.BaseDirectory + "GISAPP1.zip";
        //    //Stopwatch stopwatch = new Stopwatch();
        //    //stopwatch.Start();
        //    //Compress(rootDir, zipFile);
        //    //stopwatch.Stop();
        //    //Console.WriteLine("压缩完成，耗时：" + stopwatch.ElapsedMilliseconds + "毫秒");

        //    // SmallestSize - 85536 - 351208
        //    // Optimal - 18642 - 353490
        //    // Fastest - 11571 - 373848
        //    // NoCompression - 856 - 1046400
        //    string rootDir = AppDomain.CurrentDomain.BaseDirectory + "GISAPP";
        //    string zipFile = AppDomain.CurrentDomain.BaseDirectory + "GISAPP22.zip";
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    System.IO.Compression.ZipFile.CreateFromDirectory(rootDir, zipFile, CompressionLevel.NoCompression, false);
        //    stopwatch.Stop();
        //    Console.WriteLine("压缩完成，耗时：" + stopwatch.ElapsedMilliseconds + "毫秒");

        //    // 1 - 2842
        //    // 5 - 2809
        //    // 9 - 2724
        //    //string zipFile = AppDomain.CurrentDomain.BaseDirectory + "GISAPP9.zip";
        //    //string extractDir = AppDomain.CurrentDomain.BaseDirectory + "GISAPP9";
        //    //Stopwatch stopwatch = new Stopwatch();
        //    //stopwatch.Start();
        //    //Decompress(zipFile, extractDir);
        //    //stopwatch.Stop();
        //    //Console.WriteLine("解压完成，耗时：" + stopwatch.ElapsedMilliseconds + "毫秒");
        //    //Console.ReadKey();
        //}

        public static bool Compress(string rootDir, string zipFile)
        {
            using (FileStream fs = File.Create(zipFile))
            {
                try
                {
                    ZipOutputStream zipOS = new ZipOutputStream(fs);
                    zipOS.SetLevel(1);
                    CompressRec(rootDir, zipOS, rootDir);
                    zipOS.Finish();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        
        private static void CompressRec(string curDir, ZipOutputStream zipOS, string rootDir)
        {
            if(curDir.Last() != Path.DirectorySeparatorChar)
            {
                curDir += Path.DirectorySeparatorChar;
            }
            string[] subDirs = Directory.GetDirectories(curDir);
            string[] subFles = Directory.GetFiles(curDir);
            foreach (var subFle in subFles)
            {
                using (FileStream fs = File.OpenRead(subFle))
                {
                    try
                    {
                        string rootZip = subFle.Substring(rootDir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                        ZipEntry entry = new ZipEntry(rootZip)
                        {
                            DateTime = DateTime.Now,
                            Size = fs.Length
                        };
                        zipOS.PutNextEntry(entry);
                        // 100MB 缓冲区
                        byte[] buffer = new byte[1024 * 1024 * 100];
                        int bytesRead;
                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            zipOS.Write(buffer, 0, bytesRead);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 异常处理...
                    }
                }
            }

            foreach (var subDir in subDirs)
            {
                CompressRec(subDir, zipOS, rootDir);
            }
        }

        public static bool Decompress(string zipFile, string extractDir)
        {
            if (!Directory.Exists(extractDir))
            {
                Directory.CreateDirectory(extractDir);
            }
            using (ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(zipFile))
            {
                try
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string entryPath = Path.Combine(extractDir, entry.FullName);
                        if (entry.FullName.EndsWith("/"))
                        {
                            Directory.CreateDirectory(entryPath);
                        }
                        else
                        {
                            string entryFolderPath = Path.GetDirectoryName(entryPath);
                            if (!Directory.Exists(entryFolderPath))
                            {
                                Directory.CreateDirectory(entryFolderPath);
                            }
                            entry.ExtractToFile(entryPath, overwrite: true);
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
