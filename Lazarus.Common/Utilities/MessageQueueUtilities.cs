//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Autofac;
//using Lazarus.Common.Enum;
//using RabbitMQ.Client;

//namespace Lazarus.Common.Utilities
//{


//    public class MessageQueueUtilities
//    {
//        public static RabbitMQ.Client.IConnection _Connection;
 
//        public static RabbitMQ.Client.IConnection CreateConnectionMessageBus()
//        {
//            var hostname = AppConfigUtilities.GetAppConfig<string>("EventBusHostName");
//            var username = AppConfigUtilities.GetAppConfig<string>("EventBusUserName");
//            var password = AppConfigUtilities.GetAppConfig<string>("EventBusPassword");
//            var virturlHost = AppConfigUtilities.GetAppConfig<string>("EventBusVirtualHost");
//            var port = AppConfigUtilities.GetAppConfig<int>("EventBusPort");
//            var conn = ConnectionString();
//            var factory = new ConnectionFactory(){HostName = hostname,UserName =username,Password = password,VirtualHost = virturlHost,Port = port};
//            var connection = factory.CreateConnection();
//            return connection;
//        }
//        public static string ConnectionString()
//        {
//            var hostname = AppConfigUtilities.GetAppConfig<string>("EventBusHostName");
//            var username = AppConfigUtilities.GetAppConfig<string>("EventBusUserName");
//            var password = AppConfigUtilities.GetAppConfig<string>("EventBusPassword");
//            var virturlHost = AppConfigUtilities.GetAppConfig<string>("EventBusVirtualHost");
//            var port = AppConfigUtilities.GetAppConfig<string>("EventBusPort");
//            var con = "";
//            if(port=="5673")
//                con=string.Format("amqp://{0}:{1}@{2}:{4}/{3}", username, password, hostname, virturlHost, port);
//            else
//            {
//               con= string.Format("host={2}:{4};virtualHost={3};username={0};password={1};timeout=10", username, password, hostname, virturlHost, port);
//            }
//            return con;
//        }

//        public static void Send(EnumMessageTopic topic, string json)
//        {


//            if (_Connection == null && DI.DomainEvents._Container==null)
//                _Connection = CreateConnectionMessageBus();


//            using (var channel = _Connection.CreateModel())
//            {
//                channel.ExchangeDeclare(exchange: topic.ToString(),
//                    type: "topic");
       
            
//                var body = Encoding.UTF8.GetBytes(json);
//                channel.BasicPublish(exchange: topic.ToString(),
//                    routingKey: "",
//                    basicProperties: null,
//                    body: body);

//            }

        
           
//        }

    

//    }
//}
