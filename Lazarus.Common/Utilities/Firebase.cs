using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazarus.Common.Utilities
{
    public class FirebaseUtilities
    {
        private const string AUTHSECRET = "pnxbHSLzVjaxW6yKfvxyuwRh8jXMaszvcQGGeIT0";
        private const string BASEPATH = "https://nexus-d5d19.firebaseio.com/";
        public static FirebaseClient GetClient()
        {
            var firebaseClient = new FirebaseClient(
                  BASEPATH,
                  new FirebaseOptions
                  {
                      AuthTokenAsyncFactory = () => Task.FromResult(AUTHSECRET)
                  });
            return firebaseClient;
        }

        [Obsolete]
        public static async Task Set<T>(T obj, string path)
        {

            var _client = GetClient();
            
            //var dino = await _client
            //  .Child(path)
            //  .PostAsync(obj.ToJSON());

            var json = obj.ToJSON();
 
            
            await HttpUtilities.RequestPost(BASEPATH + path + $"?auth={AUTHSECRET}", json, null);



        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="path"> mytopic/001.json ชื่อ topic ตามด้วย id</param>
        /// <returns></returns>
        public static async Task Update<T>(T obj, string path)
        {
            await HttpUtilities.RequestPut(BASEPATH + path + $"?auth={AUTHSECRET}", obj.ToJSON(), null);

        }

        public static async Task Delete( string path)
        {
            await HttpUtilities.RequestDelete(BASEPATH + path + $"?auth={AUTHSECRET}");

        }

        //public static async Task<T> Get<T>(string path)
        //{
        //    try
        //    {
        //        var _client = GetClient();

        //        FirebaseResponse response = await _client.GetAsync(path);
        //        T r = response.ResultAs<T>();
        //        return r;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }

        //}
    }
}
