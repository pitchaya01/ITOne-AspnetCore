using Autofac;
using Lazarus.Common.Attributes;
using Lazarus.Common.DI;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lazarus.Common.Utilities
{
 
    public class EmailUtilities
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static void SendEmailWithBodyHTML(EmailModel model)
        {
            DomainEvents._Container.Resolve<ILogRepository>().Info(model.ToJSON(), "SendEmailWithBodyHTML", UserUtilities.GetUser(), UserUtilities.GetToken());
            if (model.Email_Tos == null || model.Email_Tos.Any() == false)
                return;
            SmtpClient client = new SmtpClient();
            //client.Port = AppConfigUtilities.GetAppConfig<int>("SMTP_PORT");
            //client.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
            client.Port = APPCONSTANT.ConfigSendEmail.SMTP_PORT;
            client.Host = APPCONSTANT.ConfigSendEmail.SMTP_HOST;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(AppConfigUtilities.GetAppConfig<string>("FREIGTHX_MAIL_FROM"));
            //var recipients = model.Email_To.Split(';');
            if (model.Email_Tos != null && model.Email_Tos.Any() && model.Email_Tos.Count() > 0)
            {
                foreach (var recipient in model.Email_Tos)
                {
                    msg.To.Add(new MailAddress(recipient));
                }
            }

            if (model.CCs != null && model.CCs.Any() && model.CCs.Count() > 0)
            {
                //var ccsList = model.CCs.Split(';');
                foreach (var cc in model.CCs)
                {
                    msg.CC.Add(new MailAddress(cc));
                }
            }

            msg.IsBodyHtml = true;
            msg.Subject = model.Subject;
            msg.Body = model.Body;
            AlternateView alternativeView = AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Html);
            //Add logo
            alternativeView.ContentId = "htmlView";
            alternativeView.TransferEncoding = TransferEncoding.SevenBit;
            var logo = GetEmbeddedImage("logo","", "Assets/images/scglogo1.png");
            alternativeView.LinkedResources.Add(logo);
            msg.AlternateViews.Add(alternativeView);
            //End logo

            client.Send(msg);
        }

        public static async Task SendEmailWithBodyHTMLAsync(EmailModel model, bool isChemDailyReport = false)
        {            
            DomainEvents._Container.Resolve<ILogRepository>().Info(model.ToJSON(), "SendEmailWithBodyHTMLAsync", UserUtilities.GetUser(), UserUtilities.GetToken());
            if (model.Email_Tos.IsHasValue()==false)
                 return ;
            SmtpClient client = new SmtpClient();
            //client.Port = AppConfigUtilities.GetAppConfig<int>("SMTP_PORT");
            //client.Host = ConfigurationManager.AppSettings["SMTP_HOST"];
            client.Port = APPCONSTANT.ConfigSendEmail.SMTP_PORT;
            client.Host = APPCONSTANT.ConfigSendEmail.SMTP_HOST;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(AppConfigUtilities.GetAppConfig<string>("FREIGTHX_MAIL_FROM"));
            //var recipients = model.Email_To.Split(';');
            if (model.Email_Tos.IsHasValue())
            {
                foreach (var recipient in model.Email_Tos.Where(x => string.IsNullOrEmpty(x) == false))
                {
                    msg.To.Add(new MailAddress(recipient));
                }
            }

			if (model.CCs.IsHasValue())
			{
				//var ccsList = model.CCs.Split(';');
				foreach (var cc in model.CCs.Where(x=>string.IsNullOrEmpty(x)==false))
				{
					msg.CC.Add(new MailAddress(cc));
				}
			}

			msg.IsBodyHtml = true;
            msg.Subject = model.Subject;
            msg.Body = model.Body;

			#region Send File Or PDF
			if (model.binary != null && model.binary.Any() && !string.IsNullOrEmpty(model.FileName))
			{
				Attachment att = new Attachment(new MemoryStream(model.binary), model.FileName);
				msg.Attachments.Add(att);
			}
            #endregion

            if (isChemDailyReport == false)
            {
                AlternateView alternativeView = AlternateView.CreateAlternateViewFromString(model.Body, null, MediaTypeNames.Text.Html);
                //Add logo
                alternativeView.ContentId = "htmlView";
                alternativeView.TransferEncoding = TransferEncoding.SevenBit;
                var logo = GetEmbeddedImage("logo","", "Assets/images/scglogo1.png");
                alternativeView.LinkedResources.Add(logo);
                msg.AlternateViews.Add(alternativeView);
                //End logo
            }

            await client.SendMailAsync(msg);
            //DomainEvents._Container.Resolve<ILogRepository>().Info(msg.ToJSON(), "SendEmailWithBodyHTMLAsync", UserUtilities.GetUser(), UserUtilities.GetToken());
        }

        private static LinkedResource GetEmbeddedImage(string contentId, string imagePath,string serverpath)
        {

            string mappath = Path.Combine(serverpath, imagePath);
            ContentType c = new ContentType("image/png");

            LinkedResource linkedResource1 = new LinkedResource(mappath);
            linkedResource1.ContentType = c;
            linkedResource1.ContentId = contentId;
            linkedResource1.TransferEncoding = TransferEncoding.Base64;
            return linkedResource1;
        }      
    }
}
