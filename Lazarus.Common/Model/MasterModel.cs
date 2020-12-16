using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common.Model
{
   public  class MasterModel<T,D>
    {
        public T value { get; set; }
        public string label { get; set; }
        public string ShortName { get; set; }
        public D Reference { get; set; }
    }
}
