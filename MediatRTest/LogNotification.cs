using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{
    // 通知类
    public class LogNotification : INotification
    {
        public string Content { get; set; }
    }
}
