//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Elasticsearch.Net;
//using Nest;


//namespace Lazarus.Common.Utilities
//{

//    public class ElasticSearchUtilities
//    {
//        /// <summary>
//        /// Insert Index Object ลง Es เป็นการทำ Indexใหม่ทุกครั้ง 
//        /// </summary>
//        /// <param name="obj"> Object ที่ส่งมาต้อง มี Field Id เป็น Primary Key </param>
//        public static void Insert(object obj)
//        {
//            try
//            {
//                var url = AppConfigUtilities.GetAppConfig<string>("ElasticsearchUrl");
//                var pool = new SingleNodeConnectionPool(new Uri(url));
//                var defaultIndex = obj.GetType().Name.ToLower();
//                var connectionSettings = new ConnectionSettings(pool)
//                    .DefaultIndex(defaultIndex);

//                var client = new ElasticClient(connectionSettings);



//                client.Index(obj, s => s.Index(defaultIndex));

//            }
//            catch (Exception e)
//            {
             
//            }

            
//        }
   
//    }
//}
