//using Autofac;
//using Lazarus.Common.DI;
//using Lazarus.Common.Interface;
//using Lazarus.Common.Utilities;
//using PostSharp.Aspects;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Lazarus.Common.Attributes
//{
//    [Serializable]
//    public class LoggingApiAsyncAttribute : MethodInterceptionAspect
//    {



//        private string GetMethodName(MethodBase method)
//        {
//            if (method.IsGenericMethod)
//            {
//                var genericArgs = method.GetGenericArguments();
//                var typeNames = genericArgs.Select(t => t.Name);
//                return string.Format("{0}<{1}>", method.Name, String.Join(",", typeNames));
//            }
//            string className = method.ReflectedType.Name;
//            return className + "/" + method.Name;
//        }
//        private string[] parameterNames;
//        public LoggingApiAsyncAttribute()
//        {
//            this.SemanticallyAdvisedMethodKinds = SemanticallyAdvisedMethodKinds.ReturnsAwaitable;
//        }
//        #region Public Methods and Operators

//        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
//        {
//            var aggregateExceptions = new List<Exception>();
//            int retries = 0;

//            try
//            {


    
//                string className = "", arguments = "";
//                if (args.Method != null)
//                    if (args.Method.DeclaringType != null) className = args.Method.DeclaringType.Namespace + args.Method.DeclaringType.Name;

//                StringBuilder parameterValues = new StringBuilder();

//                for (int i = 0; i < args.Arguments.Count; i++)
//                {
//                    if (parameterValues.Length > 0)
//                    {
//                        parameterValues.Append(", ");
//                    }

//                    parameterValues.AppendFormat(
//                        "{0} = {1}", parameterNames[i], args.Arguments[i].ToJSON() ?? "null");
//                }

 

//                //DomainEvents._Container.Resolve<ILogRepository>().Info(parameterValues.ToString(), GetMethodName(args.Method), UserUtilities.GetUser(), UserUtilities.GetToken());

//                await args.ProceedAsync();
               
           

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }

//        }

//        #endregion
//    }
//}
