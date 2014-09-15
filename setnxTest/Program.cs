using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace setnxTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = new CancellationTokenSource();
            var ct = ts.Token;
            ConfigurationOptions config = new ConfigurationOptions
            {
                EndPoints =
                {
                    { "jimmyMStation", 6379 }
                }
            };
            var redis = ConnectionMultiplexer.Connect(config);

            var t = Task.Factory.StartNew(() =>
            {
                setnx(redis, ct);
            }, ct);

            Console.ReadLine();
            ts.Cancel();
            Console.ReadLine();
        }

        public static void setnx(ConnectionMultiplexer redis, CancellationToken ct)
        {
            var db = redis.GetDatabase();
            TimeSpan ts = new TimeSpan(100000000);//10 second
            var times = 1;
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }
                var resFlag = db.StringSet("testTs", "value set " + times + " times", ts, When.NotExists);
                times = resFlag ? times + 1 : times;
                var hello = resFlag ? "key set is ok" : "key is exist";
                Console.WriteLine(hello);
                var res = db.StringGet("testTs");
                Console.WriteLine(res);
                Thread.Sleep(2000);
            }

            Console.WriteLine("--------set end-----------");
        }
    }
}
