//using PostSharp.Aspects;
//using System;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace Lazarus.Common.Attributes
//{
//    [Serializable]
//    public class TransactionScopeAsyncAttribute : MethodInterceptionAspect
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

//        public TransactionScopeAsyncAttribute()
//        {
//            this.SemanticallyAdvisedMethodKinds = SemanticallyAdvisedMethodKinds.ReturnsAwaitable;
//        }
//        #region Public Methods and Operators

//        //public virtual Task OnInvokeAsync(MethodInterceptionArgs args)
//        //{

//        //}
//        public override async Task OnInvokeAsync(MethodInterceptionArgs args)
//        {
//            var aggregateExceptions = new List<Exception>();
//            int retries = 0;

//            try
//            {

             
//                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
//                {
//                    await args.ProceedAsync();
//                    scope.Complete();
//                }

//            }
//            catch (Exception ex)
//            {
                
//                throw ex;
//            }

//        }

//        #endregion
//    }
//}
