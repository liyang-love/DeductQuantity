using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{

    // 处理器1
    public class FileLogger : INotificationHandler<LogNotification>
    {
        public Task Handle(LogNotification notification, CancellationToken token)
        {
            Console.WriteLine($"FileLogger Log: {notification.Content}");
            File.AppendAllText("log.txt", $"File Log: {notification.Content}");
            return Task.CompletedTask;
        }
    }
}
