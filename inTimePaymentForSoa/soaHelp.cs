using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inTimePaymentForSoa
{
    class soaHelp : ctripRedisHelpBase
    {
        public soaHelp(string redisIp, int redisPort, helpBase help) :
            base(redisIp,redisPort,help)
        {
            
        }

        public payMethod inTimePaymentResultHelp(string reqId, string payResult, payMethod syncOrNot)
        {
            var subNum = this.RedisHelp.publish(reqId, payResult);

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
