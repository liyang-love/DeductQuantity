using System.Threading.Tasks.Dataflow;

namespace TPLDataflowTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1. 创建数据流块
            var bufferBlock = new BufferBlock<int>();          // 数据缓冲区:ml-citation{ref="3,4" data="citationList"}
            var transformBlock = new TransformBlock<int, string>(n =>
                {
                    return n % 2 == 0 ? $"Even222: {n}_{Thread.CurrentThread.ManagedThreadId}" : $"Odd11: {n}_{Thread.CurrentThread.ManagedThreadId}";
                },new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 8 });      // 数据转换（同步处理）:ml-citation{ref="4,7" data="citationList"}
            var actionBlock = new ActionBlock<string>(s =>
                Console.WriteLine($"Processed: {s}"), new ExecutionDataflowBlockOptions
                {
                    MaxDegreeOfParallelism = 4               // 设置最大并行度:ml-citation{ref="2,7" data="citationList"}
                });         // 最终消费者:ml-citation{ref="3,7" data="citationList"}

            // 2. 连接管道
            bufferBlock.LinkTo(transformBlock);
            transformBlock.LinkTo(actionBlock);

            // 3. 生产数据
            for (int i = 1; i <= 10000; i++)
            {
              await  bufferBlock.SendAsync(i);  // 异步发送数据
            }

            // 4. 标记完成并等待处理结束
            
            bufferBlock.Complete();
            await transformBlock.Completion;
            await actionBlock.Completion;

        }
    }
}
