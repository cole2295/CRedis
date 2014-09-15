﻿using ctripRedisHelp;
using inTimePayServer.paySOAService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace inTimePayServer.Controllers
{
    public class inTimePayControllerHelpController : ApiController
    {
        [HttpGet]
        public string payRequestHelp(string reqId)
        {
            var ip = "jimmyMStation.local";//"172.16.144.70";
            var port = 6379;

            using (var help = helpBase.init(ip, port,new TimeSpan(0,5,0)))
            {
                var subItem = reqId;
                var waitTime = 5000;
                var res = help.inTimePaymentMethodHelp(subItem, waitTime, () =>
                {
                    var billRes = new paymentEntity();
                    using (var req = new PaymentServiceClient())
                    {
                        billRes = req.createBillHelp(new inTimeReqPara { ReqId = reqId });
                    }

                    bool flag = false;
                    if (string.IsNullOrEmpty(billRes.BillNo))
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }

                    return flag;
                });


                return res;
            }
        }
    }
}
