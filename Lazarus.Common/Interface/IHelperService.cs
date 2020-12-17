using Lazarus.Common.Enum;
using Lazarus.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common.Interface
{
    public interface IHelperService
    {
         string UploadFile(byte[] file,string filename);
         int RunningId(EnumRunningType Type);
        byte[] GetFileUpload(out FileUploadResponseViewModel response, string id);
        void Email(string mailto, string bodyHtmlString, string subject, List<string> cc, string mailFrom = "");

    }
}
