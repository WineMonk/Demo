using System.Security.Cryptography;

namespace TestMD5
{
    internal class Program
    {
        /*
         * 
         * MD5 算法原理
         * 
         *     MD5（Message - Digest Algorithm 5）是一种广泛使用的哈希函数，
         * 它可以将任意长度的数据转换为固定长度（128 位）的哈希值。
         * 对于给定的输入数据，MD5 算法会按照特定的规则进行计算，
         * 只要输入数据完全相同，计算出来的 MD5 值就相同。
         *     当压缩一个文件夹时，压缩软件会按照一定的顺序将文件夹中的文件内容、文件属性
         * （如文件名、文件大小、修改时间等）以及文件夹结构等信息转换为二进制数据进行压缩。
         * 如果两次压缩过程完全相同，包括文件夹内容没有变化、压缩软件的版本和设置相同等因素，
         * 那么生成的 zip 文件的二进制内容也是完全相同的。
         * 
         */
        static void Main(string[] args)
        {
            string filePath1 = @"D:\admin\Desktop\Package\01\2024110701.rar";
            string filePath2 = @"D:\admin\Desktop\Package\02\2024110701.rar";
            string md51 = GetMD5Hash(filePath1);
            string md52 = GetMD5Hash(filePath2);
            Console.WriteLine("MD5 of {0} is {1}", filePath1, md51);
            Console.WriteLine("MD5 of {0} is {1}", filePath2, md52);
            Console.WriteLine("MD5 of {0} and {1} are the same: {2}", filePath1, filePath2, md51 == md52);
            Console.ReadKey();
            // Output:
            // MD5 of D:\admin\Desktop\Package\01\2024110701.rar is 6a73fb39b40c1ea68feb2676c762f0fc
            // MD5 of D:\admin\Desktop\Package\02\2024110701.rar is 6a73fb39b40c1ea68feb2676c762f0fc
            // MD5 of D:\admin\Desktop\Package\01\2024110701.rar and D:\admin\Desktop\Package\02\2024110701.rar are the same: True
        }

        static string GetMD5Hash(string filePath)
        {
            const int bufferSize = 4096;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        md5.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                    }
                    md5.TransformFinalBlock(buffer, 0, 0);
                    var hash = md5.Hash;
                    string md5String = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    return md5String;
                }
            }
        }
    }
}
