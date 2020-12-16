using Lazarus.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Interface
{
    public interface ILogRepository
    {
        void Error(string message, string stracktrace = "", string module = "", UserCredential user = null, string token = "");
        void Info(string message, string module = "", string createby = "");
        void Info(string message, string module = "", UserCredential user = null, string token = "");
        void WriteLogLocal(string message, string stracktrace = "", string model = "");
    }
}
