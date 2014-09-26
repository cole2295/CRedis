using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inTimePaymentForSoa
{
    public static class soaHelp
    {

        //public static payMethod inTimePaymentResultHelp(this IhelpBase help,string reqId, string payResult, payMethod syncOrNot)
        //{
        //    var subNum = help.publish(reqId, payResult);

        //    if (subNum > 0)
        //    {
        //        return syncOrNot;
        //    }
        //    else
        //    {
        //        return payMethod.async;
        //    }
        //}

        /// <summary>
        /// help to sync the payment method and result
        /// </summary>
        /// <param name="help">ihelpbase object</param>
        /// <param name="billNo">bill number</param>
        /// <param name="payResult">the payresult Serialized as string</param>
        /// <param name="syncOrNot">the enum type to set the payment method</param>
        /// <param name="resultExpiry">how long(minutes) does result exist in redis</param>
        /// <returns>the final payment method the payService and paySoa will ack</returns>
        public static payMethod inTimePaymentResultHelpWithSetnx(this IhelpBase help, string billNo, string payResult, payMethod syncOrNot,int resultExpiry=1)
        {
            bool flag = help.setnx(billNo, payResult, new TimeSpan(0,resultExpiry,0));
            if (flag)
            {
                var subNum = help.publish(billNo, payResult);

                return syncOrNot;
            }
            else
            {
                var expiryTime = 1000;
                var ts = new TimeSpan(expiryTime);
                help.setnx(billNo, "", ts);
                return payMethod.async;
            }
        }
    }
}
