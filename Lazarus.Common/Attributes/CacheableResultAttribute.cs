using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lazarus.Common.Attributes
{
    class MethodResultCache
    {
        private readonly string _methodName;
        private readonly TimeSpan _expirationPeriod;
        private static readonly Dictionary<string, MethodResultCache> MethodCaches =
                new Dictionary<string, MethodResultCache>();

        public MethodResultCache(string methodName, int expirationPeriod = 30)
        {
            _methodName = methodName;
            _expirationPeriod = new TimeSpan(0, 0, expirationPeriod, 0);

        }

        private string GetCacheKey(IEnumerable<object> arguments)
        {
            var key = string.Format(
              "{0}({1})",
              _methodName,
              string.Join(", ", arguments.Select(x => x != null ? x.ToString() : "<Null>")));
            return key;
        }

        public void CacheCallResult(object result, List<object> arguments)
        {
          //  HttpContext.Current.Cache.Add(GetCacheKey(arguments), result, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 30, 0), System.Web.Caching.CacheItemPriority.Default, null);
        }

        public object GetCachedResult(IEnumerable<object> arguments)
        {
            return null;
         //   return HttpContext.Current.Cache.Get(GetCacheKey(arguments));
        }

        public void ClearCachedResults()
        {


        }
        public void RemoveCacheContainKey(string contain)
        {
            //var keysToClear = (from System.Collections.DictionaryEntry dict in HttpContext.Current.Cache
            //                   let key = dict.Key.ToString()
            //                   where key.Contains(contain)
            //                   select key).ToList();
            //foreach (var c in keysToClear)
            //{
            //    //HttpContext.Current.Cache.Remove(c);
            //}
        }
        public static MethodResultCache GetCache(string methodName)
        {
            if (MethodCaches.ContainsKey(methodName))
                return MethodCaches[methodName];
            var cache = new MethodResultCache(methodName);
            MethodCaches.Add(methodName, cache);
            return cache;
        }

        public static MethodResultCache GetCache(MemberInfo methodInfo)
        {
            var methodName = string.Format("{0}.{1}.{2}",
                                           methodInfo.ReflectedType.Namespace,
                                           methodInfo.ReflectedType.Name,
                                           methodInfo.Name);
            return GetCache(methodName);
        }
    }
    [Serializable]
    public class CacheableResultAttribute : MethodInterceptionAspect
    {
        
        public CacheableResultAttribute()
        {

        }
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var cache = MethodResultCache.GetCache(args.Method);
            var arguments = args.Arguments.ToList();

            var result = cache.GetCachedResult(arguments);
            if (result != null)
            {
                args.ReturnValue = result;
                return;
            }

            base.OnInvoke(args);

            cache.CacheCallResult(args.ReturnValue, arguments);

        }
    }
}
