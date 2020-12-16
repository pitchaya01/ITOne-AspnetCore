using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Lazarus.Common.DAL;
using Lazarus.Common.Utilities;
using Lazarus.Common.Enum;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;
using Lazarus.Common.Nexus.Database;
using System.Transactions;

namespace Lazarus.Common.Utilities
{
    public class LogRepository:ILogRepository
    {
        public NexusDataContext _db;

        public LogRepository(NexusDataContext db)
        {
            _db = db;
        }    
        public void Error(string message, string stracktrace = "", string module = "", UserCredential user = null, string token = "")
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var u = UserUtilities.GetUser();
                    var log = new LogMessage();
                    log.Message = message;
                    log.Level = EnumLogLevel.Error.ToString();
                    log.CreateBy = user != null ? user.UserId : "";
                    log.Token = token;
                    log.Module = module;

                    log.StackTrace = stracktrace;
                    
                    log.CreateDate = DateTime.Now;
                    if (u != null)
                    {
                        log.CreateBy = u.UserId;
                        log.Token = UserUtilities.GetToken();
                    }

                    _db.LogMessages.Add(log);
                    _db.SaveChanges();
                    scope.Complete();
                    
                }
            }
            catch (Exception e)
            {
                WriteLogLocal(message, stracktrace, module);
            }

        }
        public void Info(string message, string module = "", UserCredential user = null, string token = "")
        {
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var log = new LogMessage();
                    log.Message = message;
                    log.Level = EnumLogLevel.Info.ToString();
                    log.Module = module;
                    log.CreateBy = user != null ? user.UserId : "";
                    log.Token = token;
                    log.Domain = AppConfigUtilities.GetDomain();
                    log.CreateDate = DateTime.Now;
                    _db.LogMessages.Add(log);
                    _db.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                WriteLogLocal(message, "", module);
            }

        }
        public void Info(string message, string module = "", string CreateBy = "")
        {
            try
            {

                using (var scope = new TransactionScope(TransactionScopeOption.Suppress))
                {
                    var log = new LogMessage();
                    log.Message = message;
                    log.Level = EnumLogLevel.Info.ToString();
                    log.Module = module;
                    log.CreateBy = CreateBy;
                    log.CreateDate = DateTime.Now;
                    log.Domain = AppConfigUtilities.GetDomain();
                    _db.LogMessages.Add(log);
                    _db.SaveChanges();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                WriteLogLocal(message, "", module);
            }

        }
        public void WriteLogLocal(string message, string stracktrace = "", string model = "")
        {
            try
            {
                
                StreamWriter log;
                FileStream fileStream = null;
                DirectoryInfo logDirInfo = null;
                FileInfo logFileInfo;

                string logFilePath = AppConfigUtilities.GetAppConfig<string>("LOG_PATH"); ;
                logFilePath = logFilePath + "Log-" + DateTime.Now.ToLocalDate().GetDateStrignDDMMMYYYHHMM() + "." + "txt";
                logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                log = new StreamWriter(fileStream);
                log.WriteLine(string.Format("{3}|{0}|{1}|{2}", message, stracktrace, model, DateTime.Now.ToLocalDate().GetDateStrignDDMMMYYYHHMM()));
                log.Close();
            }
            catch (Exception e)
            {
            }

        }
    }
}
