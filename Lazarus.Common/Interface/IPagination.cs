using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common.Interface
{
    public interface IPagination
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
