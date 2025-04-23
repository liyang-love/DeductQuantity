using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{
    public class Ping : IRequest<string>
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
