//using PostSharp.Aspects;
//using System;
//using System.Linq;

//namespace Lazarus.Common.Attributes
//{
//    [Serializable]
//    public class ReoveCacheResultAttribute : MethodInterceptionAspect
//    {
//        public string CACHE_KEY { get; set; }
//        public override void OnInvoke(MethodInterceptionArgs args)
//        {
//            var cache = MethodResultCache.GetCache(args.Method);
//            var arguments = args.Arguments.ToList();

       
            
//            base.OnInvoke(args);

//            cache.RemoveCacheContainKey(CACHE_KEY);
//        }
//    }
//}
