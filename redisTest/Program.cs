using ctripRedisHelp;
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
            var ip = "172.16.144.70";
            var port = 6379;

            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var ts = new TimeSpan(0, 5, 0);

                help.setnx("aab", "cc", ts);
                var b = help.get("aab");

                var subItem = "message";
                var waitTime = 5000;
                var t = help.subscriberInTime(subItem, waitTime);

                //request paymentSOA
                var res = t.Result;

                var pubItem = "payMethod";
                var pubValue = string.Empty;
                //Thread.Sleep(30000);    
                if (string.IsNullOrEmpty(res))
                {
                    pubValue = "async";
                }
                else
                {
                    pubValue = "sync";
                }

                help.publish(pubItem, pubValue);
            }
            //Thread.Sleep(2);
            //Console.ReadLine(); 
            
            Console.ReadLine();
        }
    }
}
