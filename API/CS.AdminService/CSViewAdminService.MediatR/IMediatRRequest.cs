using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSViewAdminService.MediatR
{
    public interface IMediatRRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
