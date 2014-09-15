using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace redisPubTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ts = new CancellationTokenSource();
            var ct = ts.Token;
            var redis = ConnectionMultiplexer.Connect("jimmyMStation");

            var t = Task.Factory.StartNew(() =>
                {
                    var times = 1;
            
                    while (true)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            break;
                        }

                        var a = redis.GetDatabase().Publish("message", "publish " + times + " times");
                        Console.WriteLine("server has published " + times + " times");

                        times++;
                        Thread.Sleep(2000);
                    }
                },ct);
            
            Console.ReadLine();
            ts.Cancel();
            Console.ReadLine();
        }
    }
}
