using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Lazarus.Common.Utilities

{
    public static class CacheServiceUtilities
    {
        static readonly Object Lock = new Object();
        static ConnectionMultiplexer _connection;

        static ConnectionMultiplexer connection
        {
            get
            {
                lock (Lock)
                {
                    if (_connection == null || !_connection.IsConnected)
                    {

                        if (_connection != null)
                        {
                            _connection.Close(false);
                            _connection.Dispose();
                            _connection = null;
                        }
               
               
                            _connection = ConnectionMultiplexer.Connect(AppConfigUtilities.GetAppConfig<string>("RedisUrl"));
                        
                        
                        
                      
                    }
                    return _connection;
                }
            }
        }
        
        public static IDatabase cache = connection.GetDatabase();

        public static T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cache key is not null");
            
            return Deserialize<T>(cache.StringGet(key));
        }

        public static object Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cache key is not null");
            return Deserialize<object>(cache.StringGet(key));
        }

        public static void Delete(string key)
        {
            cache.KeyDelete(key);
        }
        public static void Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cache key is not null");
            if (value == null) throw new ArgumentException("Cache value is not null");
            cache.StringSet(key, value.ToJSON(), TimeSpan.FromDays(365));
        }
        public static void SetBinary(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cache key is not null");
            if (value == null) throw new ArgumentException("Cache value is not null");
            var obj = Serialize(value);
            cache.StringSet(key, obj, TimeSpan.FromDays(365));
        }
        public static void Set(string key, object value,TimeSpan expire)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("Cache key is not null");
            cache.StringSet(key, value.ToJSON(), expire);
        }

        static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                byte[] objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

       public static void Flush()
       {
           var cacheList = CacheServiceUtilities.Get<List<string>>("CACHEATTR");
           cacheList.ForEach(s =>
           {
               CacheServiceUtilities.Delete(s);
           });
           
           CacheServiceUtilities.Set("CACHEATTR",new List<string>());
        }

        static T Deserialize<T>(byte[] data)
        {
               if (data == null) return default(T);
            var stream = new MemoryStream(data);
            var content = new StreamReader(stream, Encoding.UTF8).ReadToEnd();

             return    Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                
        }
    }
}
