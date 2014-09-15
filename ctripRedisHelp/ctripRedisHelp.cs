using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ctripRedisHelp
{
    public class helpBase:IDisposable
    {
        static ConnectionMultiplexer redis;// = ConnectionMultiplexer.Connect("172.16.144.70");
        private string pubValue = string.Empty;

        public static helpBase init(string ip, int port)
        {
            return new helpBase(ip, port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipPort">as 0.0.0.0:0</param>
        /// <returns></returns>
        public static helpBase init(string ipPort)
        {
            return new helpBase(ipPort);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipPort">as 0.0.0.0:0</param>
        private helpBase(string ipPort)
        {
            redis = ConnectionMultiplexer.Connect(ipPort);
        }

        private helpBase(string ip,int port)
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { ip, port }
                }
            };
            redis = ConnectionMultiplexer.Connect(config);
        }

        public void setnx(string key,string value)
        {
            var db = redis.GetDatabase();
            db.StringSet(key, value, when:When.NotExists, flags:CommandFlags.FireAndForget);
        }

        public string get(string key)
        {
            var db = redis.GetDatabase();
            var res = db.StringGet(key).ToString();
            db.StringSet(key, res, expiry: new TimeSpan(1), flags: CommandFlags.FireAndForget);
            return res;
        }

        public Task<string> subscriberInTime(string SubscribeItem, int timeOut = 5000)
        {
            var t = Task.Factory.StartNew<string>(() =>
            {

                return subscribe(SubscribeItem, timeOut);
            });
            return t;
        }

        public void publish(string publishItem,string value)
        {
            redis.GetDatabase().Publish(publishItem,value);
        }

        internal string subscribe(string SubscribeItem, int timeOut = 5000)
        {
            var sub = redis.GetSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;
            var t = Thread.CurrentThread;
            var flag = true;
            sub.Subscribe(SubscribeItem, (channel, message) =>
            {
                if (flag && !message.Equals(pubValue))
                {
                    sub.Unsubscribe(SubscribeItem);
                    Console.WriteLine(message);
                    res = message.ToString();
                    //res = "evil";
                    t.Interrupt();
                }

            });

            try
            {
                Thread.Sleep(timeOut);
                flag = false;
                Console.WriteLine("timeout");
                sub.Unsubscribe(SubscribeItem);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("arrive in time");
            }

            return res;

        }

        private string subscribeSelfFilter(string SubscribeItem, int timeOut = 5000)
        {
            var sub = redis.GetSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;
            var t = Thread.CurrentThread;
            var flag = true;
            sub.Subscribe(SubscribeItem, (channel, message) =>
            {
                if (flag && !string.IsNullOrEmpty(pubValue) && message.Equals(pubValue))
                {
                    sub.Unsubscribe(SubscribeItem);
                    Console.WriteLine(message);
                    res = message.ToString();
                    //res = "evil";
                    t.Interrupt();
                }

            });

            try
            {
                Thread.Sleep(timeOut);
                flag = false;
                Console.WriteLine("timeout");
                sub.Unsubscribe(SubscribeItem);
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("arrive in time");
            }

            return res;

        }

        public string inTimePaymentMethodHelp(string reqId,int timeout,Func<bool> act)
        {
            var t = subscriberInTime(reqId, timeout);
            var flag = act();
            var res = string.Empty;
            if (flag)
            {
                res = t.Result;
                var pubValue = string.Empty;
                if (string.IsNullOrEmpty(res))
                {
                    pubValue = "async";
                }
                else
                {
                    pubValue = "sync";
                }

                publish(reqId, pubValue);
            }else
            {
               
            }
            return res;
        }

        public string inTimePaymentResultHelp(string reqId, string payResult, int timeout, Func<bool> act)
        {
            var t = subscriberInTime(reqId, timeout);
            pubValue = payResult;
            publish(reqId, pubValue);

            var flag = act();
            var res = string.Empty;
            if (flag)
            {
                res = t.Result;
            }
            else
            {
                
            }
            return res;
        }

        public void Dispose()
        {
            redis.Close();
            redis.Dispose();
        }
    }
}
