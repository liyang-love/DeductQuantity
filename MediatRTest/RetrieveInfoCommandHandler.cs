using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{
    /// <summary>
    /// 请求应答处理类
    /// </summary>
    public class RetrieveInfoCommandHandler : IRequestHandler<RetrieveInfoCommandRequest, RetrieveInfoCommandResponse>
    {
        public async Task<RetrieveInfoCommandResponse> Handle(RetrieveInfoCommandRequest request, CancellationToken cancellationToken)
        {
            var response = new RetrieveInfoCommandResponse();
            response.OutputMessage = $"This is an example of MediatR using {request.Text}";
            return response;
        }
    }
}
