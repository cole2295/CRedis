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
        public static string inTimePaymentMethodHelp(this IhelpBase help, string reqId, int timeout, Func<bool> act)
        {
            Console.WriteLine("client is waitting");
            var res = string.Empty;

            help.subscriber(reqId);

            var flag = act();
            if (flag)
            {                
                help.Wait(timeout);
                Console.WriteLine("timeout");
                res = help.InValue;
            }
            else
            {

            }

            help.unSubscriber(reqId);
            //res = help.InValue;
            return res;
        }
    }
}