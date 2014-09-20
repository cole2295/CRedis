using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace inTimePaymentForService
{
    public static class serviceHelp
    {
        public static string inTimePaymentMethodHelp(this helpBase help, string reqId, int timeout, Func<bool> act)
        {
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var sub = help.getSubscriber();
            Console.WriteLine("client is waitting");
            var res = string.Empty;

            sub.SubscribeAsync(reqId, (channel, message) =>
            {
                Console.WriteLine(message);
                res = message.ToString();
                autoEvent.Set();
            });

            var flag = act();
            if (flag)
            {
                autoEvent.WaitOne(timeout);
                Console.WriteLine("timeout");
            }
            else
            {

            }

            sub.Unsubscribe(reqId);

            return res;
        }
    }
}