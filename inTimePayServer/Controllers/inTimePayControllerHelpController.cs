using ctripRedisHelp;
using inTimePayServer.paySOAService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceStack.Text;
using ServiceStack;

namespace inTimePayServer.Controllers
{
    public class inTimePayControllerHelpController : ApiController
    {
        [HttpGet]
        public string payRequestHelp(string reqId)
        {
            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;

            var res = string.Empty;
            using (var help = helpBase.init(ip, port,new TimeSpan(0,5,0)))
            {
                var subItem = reqId;
                var waitTime = 5000;
                res = help.inTimePaymentMethodHelp(subItem, waitTime, () =>
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
                        #region read paymentResult in cache
                        //repeated payment req
                        //if (string.IsNullOrEmpty(redisValueTmp))
                        //{
                        //    //async
                        //}
                        //else
                        //{
                        //    var payInfoTmp = redisValueTmp.FromJson<Dictionary<string, string>>();
                        //    var method = payInfoTmp["payMethod"];
                        //    var result = payInfoTmp["result"];
                        
                        //    if (payMethod.sync.Equals(result))
                        //    {
                        //        //sync
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        #endregion
                    }

                    return flag;
                });

                //var payInfoTmp = res.FromJson<Dictionary<string, string>>();
                //var method = payInfoTmp["payMethod"];
                //var result = payInfoTmp["result"];
                return res;
            }
        }
    }
}
