using Lazarus.Common.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Proxy
{
    public interface  IMasterProxy
    {
        Task<ResponseResult<List<MasterModel<string, string>>>> GetCustomer();
    }
}
