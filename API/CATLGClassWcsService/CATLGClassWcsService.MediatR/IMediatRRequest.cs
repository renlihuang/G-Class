using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CATLGClassWcsService.MediatR
{
    public interface IMediatRRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
