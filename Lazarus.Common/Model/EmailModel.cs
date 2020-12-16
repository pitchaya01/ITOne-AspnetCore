using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Model
{
    public class EmailModel
    {
        public string Email_From { get; set; }
        public List<string> Email_Tos { get; set; }
        public List<string> CCs { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
		public byte[] binary { get; set; }
		public string FileName { get; set; }
	}
}
