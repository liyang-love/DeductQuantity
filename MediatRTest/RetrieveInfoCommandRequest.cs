using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{
    /// <summary>
    /// 请求类
    /// </summary>
    public class RetrieveInfoCommandRequest : IRequest<RetrieveInfoCommandResponse>
    {
        public string Text { get; set; }
    }
}
