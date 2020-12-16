//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Net.Http.Formatting;
//using System.Reflection;
//using System.Text;
//using System.Web.Http;
//using Autofac;
//using Lazarus.Common.DI;
//using Lazarus.Common.Enum;
//using Lazarus.Common.Interface;
//using Lazarus.Common.Model;
//using Lazarus.Common.Utilities;
//using PostSharp.Aspects;
//using PostSharp.Extensibility;
//namespace Lazarus.Common.Attributes
//{
//    [Serializable]
//    [MulticastAttributeUsage(MulticastTargets.Method)]

//    public class LoggingApi : PostSharp.Aspects.OnMethodBoundaryAspect
//    {
//        [NonSerialized]
//        Stopwatch _StopWatch;

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

//        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
//        {
//            parameterNames = method.GetParameters().Select(p => p.Name).ToArray();
//        }

//        public override void OnEntry(PostSharp.Aspects.MethodExecutionArgs args)
//        {
//            try
//            {
//                _StopWatch = new Stopwatch();
//                _StopWatch.Start();
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

//                _StopWatch = Stopwatch.StartNew();

//                var log = new LogMessage();
//                log.Message =  parameterValues.ToString();
//                log.Level = EnumLogLevel.Info.ToString();
//                log.Module = args.Method.DeclaringType.Name + "/" + GetMethodName(args.Method);
          
//                DomainEvents._Container.Resolve<ILogRepository>().Info(log.Message, log.Module,"");
//            }
//            catch (Exception e)
//            {
//                throw e;
//            }

//            base.OnEntry(args);
//        }

//        public override void OnException(MethodExecutionArgs args)
//        {          
//            var message = args.Exception.GetMessageError();
//            try
//            {
//                string arguments = "";
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

//                var log = new LogMessage();
//                log.Message = message + "|" + parameterValues;
//                log.Level = EnumLogLevel.Error.ToString();
//                log.Module = args.Method.DeclaringType.Name + "/" + GetMethodName(args.Method);
//                log.StackTrace = args.Exception.StackTrace;

//                DomainEvents._Container.Resolve<ILogRepository>().Error(log.Message, args.Exception.StackTrace, log.Module);
//            }
//            catch (Exception e)
//            {
//                DomainEvents._Container.Resolve<ILogRepository>().Error(e.GetMessageError(), e.StackTrace, "AttributeLogging");
//            }
//            base.OnException(args);
//        }
//    }
//}