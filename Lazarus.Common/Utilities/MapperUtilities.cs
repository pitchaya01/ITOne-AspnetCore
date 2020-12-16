
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Utilities
{
    public static class MapperExtension
    {
        public static TModel ToModel<TModel>(this object entity)where TModel:class
        {
            var json = entity.ToJSON();
            return json.ToObject<TModel>();
 
        }
    }
 
}
