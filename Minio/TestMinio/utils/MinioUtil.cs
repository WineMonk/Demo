using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Minio.DataModel.Response;
using Minio.Exceptions;
using MinioTest.entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MinioTest.utils
{
    public static class MinioUtil
    {
        private static IMinioClient _minio;
        public static IMinioClient GetMinioClient()
        {
            return _minio ??= _minio = new MinioClient()
                .WithCredentials("xUz59ICZMg5mdbmvfz2C",
                    "WHQGp0kF7BIMzdzIRzqIb9cv1UxnLZWWqs6qPnUF")
                .WithEndpoint("81.70.35.77:9000")
                .Build();
            //return _minio ??= _minio = new MinioClient()
            //    .WithCredentials("Bv8yTk82H3P3pzi50y4M",
            //        "0NaZjrsasWq9c51EfjB4Cyeg0KicGFPOB0UUVGfA")
            //    .WithEndpoint("127.0.0.1:9005")
            //    .Build();
        }

        public static async Task UploadFile(IMinioClient minio,
            string bucketName, string filePath)
        {
            var objectName = Path.GetFileName(filePath);
            var contentType = "application/zip";
            try
            {
                // Make a bucket on the server, if not already present.
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                ConsoleUtil.Info($"查询存储桶 {bucketName} ...");
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    ConsoleUtil.Info($"{bucketName} 存储桶不存在，创建存储桶...");
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                // Upload a file to bucket.
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFileName(filePath)
                    .WithContentType(contentType);
                ConsoleUtil.Info($"文件上传 - {filePath} ...");
                PutObjectResponse putObjectResponse = await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                ConsoleUtil.Info($"文件上传成功 - 【{objectName}】");
            }
            catch (MinioException e)
            {
                ConsoleUtil.Error($"文件上传失败: {e.Message}");
            }
        }
    
        public static async Task<List<FileItem>> GetFileItems(IMinioClient minio,
            string bucketName, bool recursive = false)
        {
            List<FileItem> list = new List<FileItem>();
            try
            {
                bool complate = false;
                var listArgs = new ListObjectsArgs()
                    .WithBucket(bucketName)
                    .WithRecursive(recursive);
                ConsoleUtil.Info($"查询存储桶 {bucketName} ...");
                var observable = minio.ListObjectsAsync(listArgs);
                var subscription = observable.Subscribe(
                    item => list.Add(new FileItem
                    {
                        FileName = item.Key
                    }),
                    ex => Debug.WriteLine($"OnError: {ex}"),
                    () => complate = true);
                while (!complate)
                {
                    await Task.Delay(100);
                }
                ConsoleUtil.Info($"查询存储桶 {bucketName} 对象列表成功！");
            }
            catch (Exception e)
            {
                ConsoleUtil.Error($"查询存储桶 {bucketName} 对象列表失败 - {e.Message}");
            }
            return list;
        }

        public static async Task DownloadFile(IMinioClient minio,
            string bucketName, string objectName, string savePath,
            IServerSideEncryption sse = null)
        {
            try
            {
                string fileName = Path.Combine(savePath, objectName);
                ConsoleUtil.Info($"下载存储桶 {bucketName} 对象 {objectName} ...");
                File.Delete(fileName);
                var args = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithFile(fileName)
                    .WithServerSideEncryption(sse);
                _ = await minio.GetObjectAsync(args).ConfigureAwait(false);
                ConsoleUtil.Info($"下载存储桶 {bucketName} 对象 {objectName} 成功 - {fileName}");
            }
            catch (Exception e)
            {
                ConsoleUtil.Error($"下载存储桶 {bucketName} 对象 {objectName} 失败 - {e.Message}");
            }
        }

        public static async Task RemoveFile(IMinioClient minio,
            string bucketName, string objectName, string versionId = null)
        {
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(objectName);
                var versions = "";
                if (!string.IsNullOrEmpty(versionId))
                {
                    args = args.WithVersionId(versionId);
                    versions = "，版本ID " + versionId + " ";
                }

                ConsoleUtil.Info($"删除存储桶 {bucketName} 对象 {objectName} ...");
                await minio.RemoveObjectAsync(args).ConfigureAwait(false);
                ConsoleUtil.Info($"删除存储桶 {bucketName} 对象 {objectName}{versions}成功！");
            }
            catch (Exception e)
            {
                ConsoleUtil.Info($"删除存储桶 {bucketName} 对象 {objectName} 失败 - {e.Message}");
            }
        }

    }
}
