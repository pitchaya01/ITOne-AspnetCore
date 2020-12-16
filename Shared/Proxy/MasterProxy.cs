using Lazarus.Common.Model;
using Lazarus.Common.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Proxy
{
    public class MasterProxy : IMasterProxy
    {
        public async Task<ResponseResult<List<MasterModel<string, string>>>> GetCustomer()
        {
            var result = await HttpUtilities.RequestGet<ResponseResult<List<MasterModel<string, string>>>>("", null);
            return result;
        }
    }
}
