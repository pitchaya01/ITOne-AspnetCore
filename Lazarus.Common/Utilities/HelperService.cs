using Lazarus.Common.Enum;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;
using Lazarus.Common.Nexus.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Utilities
{
    public class FileUploadResponseViewModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public float SizeMB { get; set; }
    }
    public class HelperService : IHelperService
    {
        public NexusDataContext _db;
        public HelperService(NexusDataContext db)
        {
            _db = db;
        }
        public byte[] GetFileUpload(out FileUploadResponseViewModel response, string id)
        {
            response = new FileUploadResponseViewModel();

            var file = _db.FileUpload.Where(s => s.Id == id).FirstOrDefault();


            response.FullName = file.FileName;
            response.Url =AppConfigUtilities.GetDomain()+"/api/authentication/GetFile?Id="+id;
 
            
            response.Id = file.Id;

            return file.File;
        }
        public  int RunningId(EnumRunningType Type)
        {


            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"EXECUTE RunningId '{Type.ToString()}'";
                command.CommandType = CommandType.Text;

                if(_db.Database.GetDbConnection().State !=  ConnectionState.Open)
                _db.Database.OpenConnection();

                using (var resultQuery = command.ExecuteReader())
                {
                    var entities = new List<int>();

                    resultQuery.Read();
                       var RunningId = resultQuery.GetInt32(0);
                    return RunningId;
                }
            }

     
      
        }
        public  void Email(string mailto, string bodyHtmlString, string subject, List<string> cc, string mailFrom = "")
        {
            string title = "Nexus Hotel Application";
            var credUsername = AppConfigUtilities.GetAppConfig<string>("MailServiceUserName");
            var credPassword = AppConfigUtilities.GetAppConfig<string>("MailServicePassword");
            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential(credUsername, credPassword);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            MailAddress
                maFrom = new MailAddress(mailFrom.IsNullOrEmpty() ? "pitchaya.k@dowhilestudio.info" : mailFrom, title, Encoding.UTF8),
                maTo = new MailAddress(mailto, title, Encoding.UTF8);

            MailMessage mmsg = new MailMessage(maFrom.Address, maTo.Address);
            if (cc.AnyAndNotNull())
            {
                foreach (string bccEmailId in cc.Where(a => a.IsNullOrEmpty() == false).ToList())
                {
                    mmsg.Bcc.Add(new MailAddress(bccEmailId));
                }
            }
            mmsg.Body = bodyHtmlString;
            mmsg.BodyEncoding = Encoding.UTF8;
            mmsg.IsBodyHtml = true;
            mmsg.Subject = subject;
            mmsg.SubjectEncoding = Encoding.UTF8;

            client.Send(mmsg);

        }
        public string UploadFile(byte[] file,string filename)
        {
            var f = new FileUpload();
            f.Id = Guid.NewGuid().ToString("n");
            f.File = file;
            f.Url = AppConfigUtilities.GetAppConfig<string>("GatewayUrl")+ "user/api/Authentication/GetImage?fileId=" + f.Id;
          
            f.FileName = f.Id+filename.GetExtension(); 
            _db.FileUpload.Add(f);
            _db.SaveChanges();
            return f.Url;


        }
    }
}
