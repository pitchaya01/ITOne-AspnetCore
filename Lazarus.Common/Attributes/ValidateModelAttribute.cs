using Lazarus.Common.Model;
using Lazarus.Common.Utilities;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lazarus.Common.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var allErrors = context.ModelState.Values.SelectMany(v => v.Errors);
                var errors = allErrors.Select(x => x.ErrorMessage).ToList().ListToHtmlString();
                throw new MessageError(errors);
                //    context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
