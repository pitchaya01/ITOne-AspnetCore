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
//using System.Threading;
//using System.Transactions;

//namespace Lazarus.Common.Attributes
//{

//    [Serializable]
//    public class TransactionScopeAttribute : MethodInterceptionAspect
//    {
//        #region Fields

//        public int maxRetries = 3;

//        public int retryDelay = 30;

//        #endregion

//        #region Public Properties

//        public int MaxRetries
//        {
//            get
//            {
//                return this.MaxRetries1;
//            }

//            set
//            {
//                this.MaxRetries1 = value;
//            }
//        }

//        public int RetryDelay
//        {
//            get
//            {
//                return this.retryDelay;
//            }

//            set
//            {
//                this.retryDelay = value;
//            }
//        }

//        public int MaxRetries1 { get => maxRetries; set => maxRetries = value; }

//        #endregion

//        #region Public Methods and Operators

//        public override  void OnInvoke(MethodInterceptionArgs args)
//        {
//            var aggregateExceptions = new List<Exception>();
      
//                try
//                {
//                    using (var scope = new TransactionScope())
//                    {
//                        args.Proceed();
//                        scope.Complete();
//                    }

   
//                }
//                catch (Exception ex)
//                {
//                    throw ex;
//                }
            
//        }

//        #endregion
//    }


//    [Serializable]
//    public class ReadUnCommitedAttribute : MethodInterceptionAspect
//    {


//        public override void OnInvoke(MethodInterceptionArgs args)
//        {
            
//            var aggregateExceptions = new List<Exception>();

//            try
//            {
//                using (var scope =
//                        new TransactionScope(TransactionScopeOption.RequiresNew,
//                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
//                {
//                    args.Proceed();
//                    scope.Complete();
//                }


//            }
//            catch (Exception ex)
//            {
//                throw ex;
//            }

//        }

//    }

//    [Serializable]
//    public class RetryAttribute : MethodInterceptionAspect
//    {
//        public int RetryCount = 3;
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

//        public override void OnInvoke(MethodInterceptionArgs args)
//        {

//            var aggregateExceptions = new List<Exception>();

//            try
//            {

//                var i = 1;
//                while (i<RetryCount)
//                {
//                    try
//                    {
//                        args.Proceed();
//                        var d = (dynamic)args.ReturnValue;
//                        var ex = d.Exception;
//                        if (ex != null)
//                            throw new Exception(((Exception)ex).GetMessageError());
                        
//                        break;
//                    }
//                    catch (Exception e)
//                    {
//                        var method = args.Method.DeclaringType.Name + "/" + GetMethodName(args.Method);
//                        DomainEvents._Container.Resolve<ILogRepository>().Error(e.GetMessageError(), e.StackTrace, method);
//                        if (i == RetryCount)
//                            throw e;

//                        Thread.Sleep(2000);
//                    }
           
      
//                    i++;
//                }
//            }
//            catch (Exception ex)
//            {
                
//                throw ex;
//            }

//        }

//    }
//}
