using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProradisEx1.Interface
{
    public interface ISendRequestService<TRequest, TResponse>
    {
        Task<TResponse> Send(TRequest param);
    }
}
