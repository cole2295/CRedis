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
        //public static string inTimePaymentMethodHelp(this IhelpBase help, string reqId, int timeout, Func<bool> act)
        //{
        //    Console.WriteLine("client is waitting");
        //    var res = string.Empty;

        //    help.subscriber(reqId);

        //    var flag = act();
        //    if (flag)
        //    {                
        //        help.Wait(timeout);
        //        Console.WriteLine("timeout");
        //        res = help.InValue;
        //    }
        //    else
        //    {

        //    }

        //    help.unSubscriber(reqId);
            
        //    return res;
        //}

        public static string inTimePaymentMethodHelpWithSetnx(this IhelpBase help, string billNo, int timeout,int resultExpiry = 1)
        {
            Console.WriteLine("client is waitting");
            var res = string.Empty;

            help.subscriber(billNo);
            help.InValue = help.get(billNo);

            var ts = new TimeSpan(0,0,0,0,1);

            var wait = string.IsNullOrEmpty(help.InValue) ? timeout : 0;
            
            help.Wait(wait);

            bool flag = help.setnx(billNo, payMethod.async.ToString(), new TimeSpan(0, resultExpiry, 0));
            res = help.InValue;
            if (flag)
            {
                ts = new TimeSpan(0, 1, 0); 
            }
            else
            {
                res = string.IsNullOrEmpty(res) ? help.get(billNo) : res;
            }

            help.set(billNo, res, ts);
            help.unSubscriber(billNo);
            return res;

        }
    }
}