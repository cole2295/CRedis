using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ctripRedisHelp
{
    public enum payMethod
    {
        sync,
        async
    }

    public class helpBase:IDisposable
    {
        static ConnectionMultiplexer redis;// = ConnectionMultiplexer.Connect("172.16.144.70");
        private string pubValue = string.Empty;
        private TimeSpan expiryTime = new TimeSpan(1);
        
        public bool IsConnected
        {
            get { return redis.IsConnected; }
            set { }
        }

        public ISubscriber getSubscriber()
        {
            return redis.GetSubscriber();
        }

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
            //ConnectionMultiplexer
            redis = ConnectionMultiplexer.Connect(config);

            this.expiryTime = ts;
        }

        public bool set(string key, string value, TimeSpan ts)
        {
            var db = redis.GetDatabase();
            var flag = db.StringSet(key, value, expiry: ts);
            return flag;
        }

        public bool setnx(string key,string value,TimeSpan ts)
        {
            var db = redis.GetDatabase();
            var flag = db.StringSet(key, value, expiry: ts, when: When.NotExists);
            return flag;
        }

        public string get(string key)
        {
            var db = redis.GetDatabase();
            var res = db.StringGet(key).ToString();
            //db.StringSet(key, res, expiry: new TimeSpan(1), flags: CommandFlags.FireAndForget);
            return res;
        }

        private Task<string> subscriberInTime(string SubscribeItem, int timeOut = 5000)
        {
            var t = Task.Factory.StartNew<string>(() =>
            {

                return subscribeSelfFilter(SubscribeItem, timeOut);
            });
            return t;
        }

        public long publishAndSet(string publishItem,string value)
        {
            this.pubValue = value;
            
            var setFlag = redis.GetDatabase().StringSet(publishItem, value, expiry:expiryTime, flags: CommandFlags.FireAndForget);
            var rClientNum = redis.GetDatabase().Publish(publishItem,value);
            return rClientNum;
        }

        public long publish(string publishItem, string value)
        {
            var rClientNum = redis.GetDatabase().Publish(publishItem, value);
            return rClientNum;
        }

        public void subscriber(string SubscribeItem,Action<RedisChannel,RedisValue> act)
        {
            redis.GetSubscriber().Subscribe(SubscribeItem, act);
        }

        private string subscribeSelfFilter(string SubscribeItem, int timeOut)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var sub = redis.GetSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;
            //var t = Thread.CurrentThread;
            //var flag = true;
            //Task realT = null;
            sub.SubscribeAsync(SubscribeItem, (channel, message) =>
            {
                //sub.Unsubscribe(SubscribeItem);
                Console.WriteLine(message);
                res = message.ToString();
                //res = "evil";
                //t.Interrupt();
                autoEvent.Set();
            });


            autoEvent.WaitOne(timeOut);
            //Thread.Sleep(timeOut);
            //flag = false;
            Console.WriteLine("timeout");

            sub.Unsubscribe(SubscribeItem);
            //if(realT != null)
            //{
            //    realT.Wait();
            //}

            //sub.Unsubscribe(SubscribeItem);



            return res;

        }

        public void Dispose()
        {
            //redis.Close();
            redis.Dispose();
        }
    }
}
