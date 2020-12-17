using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common.Interface
{
    public interface IPagination
    {
         int PageIndex { get; set; }
         int PageSize { get; set; }
    }
}
