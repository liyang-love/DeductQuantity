using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 初始化试剂库存
            List<Reagent> reagents = new List<Reagent>
                {
                    new Reagent { Name = "试剂A", BatchNumber = "B001", ManufactureDate = new DateTime(2023, 1, 1), ExpiryDate = new DateTime(2024, 1, 1), Location = "仓库1" },
                    new Reagent { Name = "试剂B", BatchNumber = "B002", ManufactureDate = new DateTime(2023, 2, 1), ExpiryDate = new DateTime(2024, 2, 1), Location = "仓库2" },
                    new Reagent { Name = "试剂C", BatchNumber = "B003", ManufactureDate = new DateTime(2023, 3, 1), ExpiryDate = new DateTime(2024, 3, 1), Location = "仓库3" },
                    new Reagent { Name = "试剂D", BatchNumber = "B004", ManufactureDate = new DateTime(2023, 4, 1), ExpiryDate = new DateTime(2024, 4, 1), Location = "仓库4" },
                };

            // 扣减库存
            string targetBatch = "B001";

            Reagent targetReagent = reagents.Find(r => r.BatchNumber == targetBatch);
            if (targetReagent == null)
            {
                Console.WriteLine($"未找到批号为 {targetBatch} 的试剂！");
                return;
            }

            //int successCount1 =1;
            //for (int i = 0; i < 10000; i++)
            //{
            //    if (i % 5 == 0)
            //    {
            //        Interlocked.Increment(ref successCount1);
            //    }
            //}
            //Console.WriteLine($"成功操作总数：{successCount1}");
            //return;

            int totalOperations = 10000;
            int successCount = 0;
            int failureCount = 0;

            Parallel.For(0, totalOperations, (i) =>
            {
                bool success = false;
                if (i % 5 == 0)
                {
                    success = targetReagent.RechargeQuantity(1);
                }
                else { targetReagent.TryDeductQuantity(1); }
                if (success)
                {
                    Interlocked.Increment(ref successCount);
                }
                else
                {
                    Interlocked.Increment(ref failureCount);
                }
            });

            Console.WriteLine($"批号 {targetBatch} 的最终库存：{targetReagent.Quantity}");
            Console.WriteLine($"充值操作总数：{successCount}");
            Console.WriteLine($"扣减操作总数：{failureCount}");
            Console.ReadKey();
        }
    }
    class Reagent
    {
        private long _quantityWithVersion; // 高 32 位存储版本号，低 32 位存储库存值

        public string Name { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Location { get; set; }
        public int Quantity
        {
            get => (int)(_quantityWithVersion & 0xFFFFFFFF); // 提取低 32 位的库存值
            private set
            {
                long currentVersion = _quantityWithVersion >> 32; // 提取高 32 位的版本号
                _quantityWithVersion = (currentVersion << 32) | (uint)value; // 更新库存值并保留版本号
            }
        }

        public Reagent()
        {
            _quantityWithVersion = ((long)0 << 32) | 10000; // 初始化版本号为 0，库存为 10000
        }

        public bool TryDeductQuantity(int amount)
        {
            if (amount <= 0) return true; // 无需操作或无效操作

            long originalValue, newValue;
            do
            {
                originalValue = _quantityWithVersion;
                int currentQuantity = (int)(originalValue & 0xFFFFFFFF); // 提取低 32 位的库存值
                int currentVersion = (int)(originalValue >> 32); // 提取高 32 位的版本号

                int newQuantity = currentQuantity - amount;
                if (newQuantity < 0)
                {
                    return false; // 防止库存为负
                }

                // 计算新的值，版本号加 1
                newValue = ((long)(currentVersion + 1) << 32) | (uint)newQuantity;

                // 使用 Interlocked.CompareExchange 确保原子性操作
            } while (Interlocked.CompareExchange(ref _quantityWithVersion, newValue, originalValue) != originalValue);

            return true;
        }

        public bool RechargeQuantity(int amount)
        {
            if (amount <= 0) return false; // 无效充值

            long originalValue, newValue;
            do
            {
                originalValue = _quantityWithVersion;
                int currentQuantity = (int)(originalValue & 0xFFFFFFFF); // 提取低 32 位的库存值
                int currentVersion = (int)(originalValue >> 32); // 提取高 32 位的版本号

                int newQuantity = currentQuantity + amount;
                // 计算新的值，版本号加 1
                newValue = ((long)(currentVersion + 1) << 32) | (uint)newQuantity;

                // 使用 Interlocked.CompareExchange 确保原子性操作
            } while (Interlocked.CompareExchange(ref _quantityWithVersion, newValue, originalValue) != originalValue);

            return true;
        }
    }

}
