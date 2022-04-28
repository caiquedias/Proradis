using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProradisEx1.Interface
{
    interface IProcessDataService<TRequest, TResponse>
    {
        Task<TResponse> Process();
    }
}
