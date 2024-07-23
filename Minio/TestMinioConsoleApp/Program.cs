using Minio;
using Minio.DataModel;

namespace TestMinioConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 初始化Minio客户端
            var minioClient = new MinioClient()
                .WithCredentials("Bv8yTk82H3P3pzi50y4M",
                    "0NaZjrsasWq9c51EfjB4Cyeg0KicGFPOB0UUVGfA")
                .WithEndpoint("127.0.0.1:9005")
                .Build();

            var bucketName = "test-resumable-upload"; // 存储桶名称
            var objectName = "docker-kibana-8-14-3.tar.gz"; // 对象名称
            var filePath = "docker-kibana-8-14-3.tar.gz"; // 文件路径
            long partSize = 5 * 1024 * 1024;

            // 检查存储桶是否存在，不存在则创建
            var found = await minioClient.BucketExistsAsync(new Minio.DataModel.Args.BucketExistsArgs().WithBucket(bucketName));
            if (!found)
            {
                await minioClient.MakeBucketAsync(new Minio.DataModel.Args.MakeBucketArgs().WithBucket(bucketName));
            }

            // 获取文件信息
            var fileInfo = new FileInfo(filePath);
            long fileLength = fileInfo.Length;
            long uploadedLength = 0;

            // 检查对象是否存在以及已上传的长度
            ObjectStat stat = null;
            try
            {
                stat = await minioClient.StatObjectAsync(new Minio.DataModel.Args.StatObjectArgs().WithBucket(bucketName).WithObject(objectName));
            }
            catch(Exception ex)
            {

            }
            if (stat != null)
            {
                uploadedLength = stat.Size;
            }

            // 使用断点续传上传文件
            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(uploadedLength, SeekOrigin.Begin); // 移动到已上传的末尾
                await minioClient.PutObjectAsync(
                    new Minio.DataModel.Args.PutObjectArgs()
                    .WithVersionId("01")
                    .WithBucket(bucketName)
                    .WithObject(objectName)
                    .WithStreamData(fileStream)
                    .WithContentType("application/octet-stream")
                    .WithObjectSize(fileLength - uploadedLength)
                    .WithProgress(new Progress<ProgressReport>(pr => Console.WriteLine($"Progress: {pr.Percentage}%"))));
            }

            Console.WriteLine("文件上传完成。");
        }
    }
}
