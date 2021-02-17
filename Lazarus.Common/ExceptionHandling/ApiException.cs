using Autofac;
using Lazarus.Common.DI;
using Lazarus.Common.Interface;
using Lazarus.Common.Model;
using Lazarus.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Lazarus.Common.ExceptionHandling
{
    public class ApiExceptionAttribute : ExceptionFilterAttribute
    {

        public override void OnException(ExceptionContext actionExecutedContext)
        {
            var msg = string.Empty;
            msg = actionExecutedContext.Exception.GetMessageError();

            actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            actionExecutedContext.HttpContext.Response.ContentType = "application/json";
            var errorModel = ResponseResult<bool?>.Fail(msg);
            if (actionExecutedContext.Exception != null)
            {

                if (actionExecutedContext.Exception.GetType() == typeof(UnauthorizedAccessException))
                {

                    DomainEvents._Container.Resolve<ILogRepository>().Error(msg, actionExecutedContext.HttpContext.Request.Path, "UNAUTH");
                    actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                }


                else if (actionExecutedContext.Exception.GetType() == (typeof(Exception)))
                {

                    DomainEvents._Container.Resolve<ILogRepository>().Error(msg, actionExecutedContext.HttpContext.Request.Path.ToString(), "SYSTEM_ERROR");

                    actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    actionExecutedContext.Result = new JsonResult(errorModel);

                }
                else if (actionExecutedContext.Exception.GetType() == (typeof(MessageError)))
                {
                    var m = (MessageError)actionExecutedContext.Exception;

                    DomainEvents._Container.Resolve<ILogRepository>().Error(msg, actionExecutedContext.HttpContext.Request.Path.ToString(), "SYSTEM_ERROR");
                    if (m.Errors.AnyAndNotNull())
                    {
                        var error = ResponseResult<List<string>>.Fail(m.Errors);
                        actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        actionExecutedContext.Result = new JsonResult(errorModel);
                    }
                    else
                    {
                        actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                        actionExecutedContext.Result = new JsonResult(errorModel);
                    }

                }
                else
                {

                    DomainEvents._Container.Resolve<ILogRepository>().Error(msg, actionExecutedContext.HttpContext.Request.Path.ToString(), "MESSAGE_ERROR");
                    actionExecutedContext.Result = new JsonResult(errorModel);
             
                }
                base.OnException(actionExecutedContext);
            }
        }
    }
}
