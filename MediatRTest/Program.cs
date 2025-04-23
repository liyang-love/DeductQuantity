using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MediatRTest
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


            // IServiceCollection负责注册
            IServiceCollection services = new ServiceCollection();

          
            //注册MediatR服务，用于测试MediatR的服务
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            //services.AddTransient<INotificationHandler<LogNotification>, ConsoleLogger>();
            //services.AddTransient<INotificationHandler<LogNotification>, FileLogger>();
            //services.AddTransient<IRequestHandler<Ping, string>, PingHandler>();
            //services.AddTransient<INotificationHandler<LogNotification>, FileLogger>();
            var provider = services.BuildServiceProvider();
            //存储全局IServiceProvider的接口实例, 便于后续获得接口实例
            ServiceLocator.ConfigService(provider);

            var mediator = provider.GetRequiredService<IMediator>();



            Task<string> task = mediator.Send(new Ping { Title = "testTitle" });
            string result = task.Result;

            await mediator.Publish(new LogNotification { Content = "System started" });

            //应答处理
            var outputMessage = await mediator.Send(new RetrieveInfoCommandRequest
            {
                Text = "测试应答"
            });
            Console.WriteLine($"测试应答_:{outputMessage.OutputMessage}");

            Console.ReadKey();
        }
    }
}
