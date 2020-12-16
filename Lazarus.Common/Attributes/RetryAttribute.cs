using Autofac;
using Lazarus.Common.DI;
using Lazarus.Common.Interface;
using Lazarus.Common.Utilities;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Lazarus.Common.Attributes
{
    [Serializable]
    public class RetryAttribute : MethodInterceptionAspect
    {
        public int RetryCount;
        public RetryAttribute(int _RetryCount=3)
        {
            RetryCount = _RetryCount;
        }
        private string GetMethodName(MethodBase method)
        {
            if (method.IsGenericMethod)
            {
                var genericArgs = method.GetGenericArguments();
                var typeNames = genericArgs.Select(t => t.Name);
                return string.Format("{0}<{1}>", method.Name, String.Join(",", typeNames));
            }
            string className = method.ReflectedType.Name;
            return className + "/" + method.Name;
        }

        public override void OnInvoke(MethodInterceptionArgs args)
        {

            var aggregateExceptions = new List<Exception>();

            try
            {

                var i = 1;
                while (i < RetryCount)
                {
                    try
                    {
                        args.Proceed();
                        var d = (dynamic)args.ReturnValue;
                        var ex = d.Exception;
                        if (ex != null)
                            throw new Exception(((Exception)ex).GetMessageError());

                        break;
                    }
                    catch (Exception e)
                    {
                        var method = args.Method.DeclaringType.Name + "/" + GetMethodName(args.Method);
                        DomainEvents._Container.Resolve<ILogRepository>().Error(e.GetMessageError(), e.StackTrace, method);
                        if (i == RetryCount)
                            throw e;
                   


                        Thread.Sleep(2000);
                    }


                    i++;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
