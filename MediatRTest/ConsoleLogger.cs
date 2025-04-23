using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{

    // 处理器2
    public class ConsoleLogger : INotificationHandler<LogNotification>
    {
        public async Task Handle(LogNotification notification, CancellationToken token)
        {
            await Task.Delay(3000);
            Console.WriteLine($"Console Log: {notification.Content}");
        }
    }
}
