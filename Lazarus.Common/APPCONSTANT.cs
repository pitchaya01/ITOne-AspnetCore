using System;
using System.Collections.Generic;
using System.Text;

namespace Lazarus.Common
{
    public static class APPCONSTANT
    {
        public static class EXCEPTION
        {
            public static int BUSINESS_CASE = 422;
            public static int BAD_REQUEST = 400;
            public static int FORBIDDEN = 403;
            public static int SYSTEM_ERROR = 500;
        }
        public static class Role
        {
            public const string USER_SCGL = "USER_SCGL";
            public const string USER_CUSTOMER = "USER_CUSTOMER";
            public const string SCGL_JOB_DELETE = "SCGL_JOB_DELETE";
            public const string SCGL_JOB_MANAGE = "SCGL_JOB_MANAGE";
            public const string SCGL_JOB_REQUEST_MAINTAIN = "SCGL_JOB_REQUEST_MAINTAIN";
        }
        public class ConfigSendEmail
        {
            public const string URL_PRODUCTION = "https://freightx.scglogistics.co.th";
            public const string SIMPLE_TEMPLATE_PATH = "~\\App_Data\\EmailTemplate\\Ebidding_Invitaion\\SIMPLE_TEMPLATE.html";

            public const string FREIGTHX_MAIL_FROM = "SCGL_FreightX@scg.com";
            public const string SMTP_HOST = "outmail.scg.co.th";
            public const int SMTP_PORT = 25;

            public class CreateUser
            {
                public const string TEMPLATE_PATH = SIMPLE_TEMPLATE_PATH;
                public const string MAIL_FROM = FREIGTHX_MAIL_FROM;
                public const string MAIL_SUBJECT = "[SCG Logistics Ebidding System] Your password has been initial/reset";
                public const string MAIL_BODY = "Your password has been initial/reset. Your new information are: \n\nUsername : {0}\nPassword : {1}\nDomain : External \n\nPlease do not reply to this mail address. If you have questions about this notification, please contact SCG Logistics administrator.";

            }
            public class ResetPasswordByFreightUser
            {
                public const string TEMPLATE_PATH = SIMPLE_TEMPLATE_PATH;
                public const string MAIL_FROM = FREIGTHX_MAIL_FROM;
                public const string MAIL_SUBJECT = "[SCG Logistics Ebidding System] Your password has been initial/reset";
                public const string MAIL_BODY = "Your password has been initial/reset. Your new information are: \n\nUsername : {0}\nPassword : {1}\nDomain : External \n\nPlease do not reply to this mail address. If you have questions about this notification, please contact SCG Logistics administrator.";
            }

            public class ResetPasswordForExternalUser
            {
                public const string TEMPLATE_PATH = SIMPLE_TEMPLATE_PATH;
                public const string MAIL_FROM = FREIGTHX_MAIL_FROM;
                public const string MAIL_SUBJECT = "[SCG Logistics Ebidding System] Link for reset password";
                public const string MAIL_BODY = "Dear {0},\n\n         Please click {1} for set new password.";
                public const string PATH_AND_QUERY_STRING = "/#/resetpasswordexternal?systemId=";
            }

            public class InvitationBidding
            {
                public const string TEMPLATE_PATH = SIMPLE_TEMPLATE_PATH;
                public const string MAIL_FROM = FREIGTHX_MAIL_FROM;
            }

            public static class EmailTemplatePurpose
            {
                public const string NEW_JOB = "NEW_JOB";
                public const string CANCEL_JOB = "CANCEL_JOB";
                public const string RESETPASSWORD_EXTERNAL = "RESETPASSWORD_EXTERNAL";
                public const string RESETPASSWORD_INTERNAL = "RESETPASSWORD_INTERNAL";
            }


        }

    }
}
