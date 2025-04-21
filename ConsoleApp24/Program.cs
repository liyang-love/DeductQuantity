using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp24
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            await PartitionerDemoAsync();
            Console.WriteLine("Hello, World!");

            Console.ReadKey();
        }

        public static string ToMD5Hash(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            var bytes = Encoding.ASCII.GetBytes(str);
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }
            using (var md5 = MD5.Create())
            {
                return string.Join("", md5.ComputeHash(bytes).Select(x => x.ToString("X2")));
            }
        }
        static async Task<List<string>> PartitionA(IEnumerator<string> partition)
        {
            using (partition)
            {
                var list = new List<string>();
                while (partition.MoveNext())
                {
                    Console.WriteLine($"当前线程ID2222:{Thread.CurrentThread.ManagedThreadId}======={list.Count}========");
                    list.Add(ToMD5Hash(partition.Current));
                }
                Console.WriteLine($"当前线程ID:{Thread.CurrentThread.ManagedThreadId}======={list.Count}========");
                return await Task.FromResult(list);
            }

        }



        static async Task PartitionerDemoAsync()
        {
            while (true)
            {
                Console.ReadLine();
                var source = new List<string>();
                for (var i = 0; i < 80000; i++)
                {
                    source.Add($"{i}{DateTime.Now.ToString("yyyyMMddHHmmssfffffff")}");
                }


                var list = Partitioner
                          .Create(source)
                          .GetPartitions(12)
                .AsParallel()
                          .Select(PartitionA);


                var count = 0;
                foreach (var item in list)
                {
                    count++;
                    foreach (var t in await item)
                    {
                        Console.WriteLine($"---{count}---{t}-----");
                    }
                }
            }
        }
    }
}
