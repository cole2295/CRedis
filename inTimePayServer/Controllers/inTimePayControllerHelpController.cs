using ctripRedisHelp;
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
        //inTimeReqPara para, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok"
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
                    var billRes = new inTimePayServer.Controllers.paySOAController.paymentEntity();
                    //using (var req = new PaymentServiceClient())
                    //{
                    //    billRes = req.createBillHelp(new inTimeReqPara { ReqId = reqId });
                    //}

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

        public inTimePayServer.Controllers.paySOAController.paymentEntity realPayRequestHelp(string reqId, int waitTime = 5000, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            //var billRes = new paymentEntity();
            //using (var req = new PaymentServiceClient())
            //{
            //    billRes = req.createBillHelp(new inTimeReqPara { ReqId = reqId });
            //}

            //return billRes;
            return new paySOAController.paymentEntity();
        }

        [HttpGet]
        public string realPayRequestHelp(string reqId, Func<inTimePayServer.Controllers.paySOAController.paymentEntity> req, int waitTime = 5000, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;

            var res = string.Empty;
            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var subItem = reqId;
                res = help.inTimePaymentMethodHelp(subItem, waitTime, () =>
                {
                    var billRes = req();

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
