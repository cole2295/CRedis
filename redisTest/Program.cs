
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redisTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //not sugested
            //var popRes = brpop(redis);
            var ip = "jimmyMStation";
            var port = 6379;

            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { ip, port }
                }
            };
            //ConnectionMultiplexer
               var subItem = "message";
                var waitTime = 1000;
                
            var redis = ConnectionMultiplexer.Connect(config);
            var t = subscribeSelfFilter(redis, subItem, waitTime, (e) => true);
            
               //var r = redi help.publish(subItem, "aa");
                
                //request paymentSOA
                var res = t;

                //var pubItem = "payMethod";
                //var pubValue = string.Empty;
                ////Thread.Sleep(30000);    
                //if (string.IsNullOrEmpty(res))
                //{
                //    pubValue = "async";
                //}
                //else
                //{
                //    pubValue = "sync";
                //}

                //help.publish(pubItem, pubValue);
            
            //Thread.Sleep(2);
            //Console.ReadLine(); 
            
            Console.ReadLine();
        }

        static string subscribeSelfFilter(ConnectionMultiplexer redis, string SubscribeItem, int timeOut, Func<RedisValue, bool> filter)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);

            
            var sub = redis.GetSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;
            var t = Thread.CurrentThread;
            
            var flag = true;
            var tt2 = DateTime.MinValue.Ticks;
            var tt1 = DateTime.MaxValue.Ticks;
            //Task realT = null;
            sub.SubscribeAsync(SubscribeItem, (channel, message) =>
            {
                if (flag && filter(message))
                {
                    //sub.Unsubscribe(SubscribeItem);
                    Console.WriteLine(message);
                    res = message.ToString();
                    //res = "evil";
                    autoEvent.Set();
                }
                tt2 = DateTime.Now.Ticks;
                Console.WriteLine("processTime:" + (tt2 - tt1));
            });

           
                //flag = false;
                Console.WriteLine("timeout");
                autoEvent.WaitOne(1);
                //Thread.Sleep(timeOut);

                tt1 = DateTime.Now.Ticks;
                //sub.Publish(SubscribeItem, "aa");
                 sub.UnsubscribeAllAsyncBefore().Wait();
                 Console.WriteLine("res:" + res);
                 var tt3 = DateTime.Now.Ticks;
                 Console.WriteLine("pubTime:" + (tt3 - tt1));
                 //Thread.Sleep(500);
                //redis.Close(false);
                //if(realT != null)
                //{
                //    realT.Wait();
                //}

                //sub.Unsubscribe(SubscribeItem);

            

            return res;

        }
    }
}
