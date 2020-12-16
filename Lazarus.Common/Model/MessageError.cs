using Lazarus.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Model
{
    public class MessageError : Exception
    {

        public List<string> Errors { get; set; }
        public MessageError(string messageError)
            : base(messageError)
        {

        }


        public MessageError(List<string> errors)
    : base(errors.ListToString())
        {
            this.Errors = errors;
        }

    }

}
