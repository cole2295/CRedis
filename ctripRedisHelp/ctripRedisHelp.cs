using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Text;
using ServiceStack;

namespace ctripRedisHelp
{
    public class helpBase:IDisposable
    {
        static ConnectionMultiplexer redis;// = ConnectionMultiplexer.Connect("172.16.144.70");
        private string pubValue = string.Empty;
        private TimeSpan expiryTime = new TimeSpan(1);

        public static helpBase init(string ip, int port,TimeSpan ts)
        {
            return new helpBase(ip, port, ts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipPort">as 0.0.0.0:0</param>
        /// <returns></returns>
        internal static helpBase init(string ipPort)
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

        private helpBase(string ip,int port,TimeSpan ts)
        {
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { ip, port }
                }
            };
            redis = ConnectionMultiplexer.Connect(config);

            this.expiryTime = ts;
        }

        public void setnx(string key,string value,TimeSpan ts)
        {
            var db = redis.GetDatabase();
            db.StringSet(key, value, expiry: ts, when: When.NotExists);
        }

        public string get(string key)
        {
            var db = redis.GetDatabase();
            var res = db.StringGet(key).ToString();
            //db.StringSet(key, res, expiry: new TimeSpan(1), flags: CommandFlags.FireAndForget);
            return res;
        }

        public Task<string> subscriberInTime(string SubscribeItem, int timeOut = 5000)
        {
            var t = Task.Factory.StartNew<string>(() =>
            {

                return subscribeSelfFilter(SubscribeItem, timeOut, (ele) => true);
            });
            return t;
        }

        public void publish(string publishItem,string value)
        {
            this.pubValue = value;
            
            redis.GetDatabase().StringSet(publishItem, value, expiry:expiryTime, flags: CommandFlags.FireAndForget);
            redis.GetDatabase().Publish(publishItem,value);
        }


        private string subscribeSelfFilter(string SubscribeItem, int timeOut, Func<RedisValue, bool> filter)
        {
            var sub = redis.GetSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;
            var t = Thread.CurrentThread;
            var flag = true;
            
            sub.Subscribe(SubscribeItem, (channel, message) =>
            {
                if (flag && filter(message))
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
            var t = Task.Factory.StartNew<string>(() =>
            {
                return subscribeSelfFilter(reqId, timeout, (ele) => !ele.Equals(pubValue));
            });

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

        public string inTimePaymentResultHelp(string reqId, string payResult, int timeOut, Func<bool> act)
        {
            var t = Task.Factory.StartNew<string>(() =>
            {
                return subscribeSelfFilter(reqId, timeOut, (ele) => !ele.Equals(pubValue));
            });
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
