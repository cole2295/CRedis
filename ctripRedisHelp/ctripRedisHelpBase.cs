using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ctripRedisHelp
{
    public class ctripRedisHelpBase
    {
        string ip = string.Empty;
        int port = 0;
        helpBase redisHelp = null;

        public helpBase RedisHelp
        {
            get { return redisHelp; }
            set { redisHelp = value; }
        }

        public ctripRedisHelpBase(string redisIp, int redisPort, helpBase help)
        {
            this.ip = redisIp;
            this.port = redisPort;
            this.redisHelp = help;
        }
    }
}
