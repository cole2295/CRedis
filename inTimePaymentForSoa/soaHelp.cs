using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inTimePaymentForSoa
{
    public static class soaHelp
    {

        public static payMethod inTimePaymentResultHelp(this IhelpBase help,string reqId, string payResult, payMethod syncOrNot)
        {
            var subNum = help.publish(reqId, payResult);

            if (subNum > 0)
            {
                return syncOrNot;
            }
            else
            {
                return payMethod.async;
            }
        }
    }
}
