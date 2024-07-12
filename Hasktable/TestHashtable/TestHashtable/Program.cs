using System.Collections;
using System.Diagnostics;

namespace TestHashtable
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建与插入对比
            List<string> guids = Enumerable.Range(1, 10000000).Select(i => Guid.NewGuid().ToString()).ToList();
            Hashtable hashtable = new Hashtable();
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (string guid in guids)
            {
                hashtable.Add(guid, guid);
            }
            stopwatch.Stop();
            Console.WriteLine($"Hashtable - 1千万条不重复数据插入耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            stopwatch.Restart();
            stopwatch.Start();
            foreach (string guid in guids)
            {
                dictionary.Add(guid, guid);
            }
            stopwatch.Stop();
            Console.WriteLine($"Dictionary - 1千万条不重复数据插入耗时: {stopwatch.ElapsedMilliseconds} 毫秒");


            List<string> guids1 = Enumerable.Range(1, 1000000).Select(i => Guid.NewGuid().ToString()).ToList();
            List<string> guids2 = Enumerable.Range(1, 10000000).Select(i =>
            {
                int idx = i % 1000000;
                return guids1[idx];
            }).ToList();

            //// 创建并填充Hashtable
            //Hashtable hashtable = new Hashtable();
            //Dictionary<int,int> dictionary = new Dictionary<int,int>();
            //for (int i = 0; i < 10000000; i++)
            //{
            //    hashtable[i] = i;
            //    dictionary[i] = i;
            //}

            //// 基准测试
            //Stopwatch stopwatch = new Stopwatch();

            //// 测试foreach遍历
            //stopwatch.Start();
            //foreach (DictionaryEntry entry in hashtable)
            //{
            //    var key = entry.Key;
            //    var value = entry.Value;
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Hashtable - foreach遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            //stopwatch.Restart();
            //stopwatch.Start();
            //foreach (KeyValuePair<int,int> entry in dictionary)
            //{
            //    var key = entry.Key;
            //    var value = entry.Value;
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Dictionary - foreach遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            //// 测试Keys遍历
            //stopwatch.Restart();
            //foreach (var key in hashtable.Keys)
            //{
            //    var value = hashtable[key];
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Hashtable - Keys遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            //stopwatch.Restart();
            //foreach (var key in dictionary.Keys)
            //{
            //    var value = dictionary[key];
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Dictionary - Keys遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            //// 测试IDictionaryEnumerator遍历
            //stopwatch.Restart();
            //IDictionaryEnumerator enumeratorHt = hashtable.GetEnumerator();
            //while (enumeratorHt.MoveNext())
            //{
            //    var key = enumeratorHt.Key;
            //    var value = enumeratorHt.Value;
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Hashtable - IDictionaryEnumerator遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");

            //stopwatch.Restart();
            //IDictionaryEnumerator enumeratorDic = dictionary.GetEnumerator();
            //while (enumeratorDic.MoveNext())
            //{
            //    var key = enumeratorDic.Key;
            //    var value = enumeratorDic.Value;
            //}
            //stopwatch.Stop();
            //Console.WriteLine($"Dictionary - IDictionaryEnumerator遍历耗时: {stopwatch.ElapsedMilliseconds} 毫秒");
        }
    }
}
