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
using System.Transactions;

namespace Lazarus.Common.Attributes
{


    [Serializable]
    public class ReadUnCommitedAttribute : MethodInterceptionAspect
    {


        public override void OnInvoke(MethodInterceptionArgs args)
        {

 
            try
            {
                using (var scope =
                        new TransactionScope(TransactionScopeOption.RequiresNew,
                        new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    args.Proceed();
                    scope.Complete();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }


}
