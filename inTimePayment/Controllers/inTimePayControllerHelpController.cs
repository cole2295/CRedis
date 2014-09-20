using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceStack;
using ServiceStack.Text;
using inTimePaymentForService;

namespace inTimePayment.Controllers
{
    public class inTimePayControllerHelp
    {
        public inTimeRes realPayRequestHelp(string reqId, Func<payMethod, int, string, paymentEntity> req, int waitTime = 5000, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;

            var res = string.Empty;
            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                res = help.inTimePaymentMethodHelp(reqId, waitTime, () =>
                {
                    var billRes = req(method, processTime, result);
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
                return res.FromJson<inTimeRes>();
            }
        }
    }

    [RoutePrefix("api/inTimePay")]
    public class inTimePayControllerHelpController : ApiController
    {
        [Route("payRequestHelp")]
        [HttpGet]
        public inTimeRes payRequestHelp(string reqId)
        {
            return new inTimePayControllerHelp().realPayRequestHelp(reqId, (pm, pt, rt) =>
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://win8dev");
                        client.DefaultRequestHeaders.Accept.Add(
                            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        var resp = client.PostAsJsonAsync("inTimePayment/api/paySOA/createBillHelp", new inTimeReqPara { ReqId = reqId }).Result;
                        var resContent = resp.Content.ReadAsAsync<paymentEntity>();
                        //using (var req = new BankServiceClient())
                        //{
                        //    req.processPaymentHelp(be);
                        //}
                        ////using (var req = new PaymentServiceClient())
                        //{
                        //    req.paymentResultHookHelp(res);
                        //}
                        return resContent.Result;
                    }
                });
        }



        [Route("payRequestHelpForTest")]
        [HttpPost]
        public inTimeRes payRequestHelpForTest([FromUri]string reqId, testParameter testPara)//int waitTime = 5000, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            return new inTimePayControllerHelp().realPayRequestHelp(reqId, (pm, pt, rt) =>
                 {
                     var forCreateBill = new createBillParaWarp { Para = new inTimeReqPara { ReqId  = reqId}, Method = pm, BankPara = new BankParaWarp { ProcessTime = pt, Result = rt } };
                     using (var client = new HttpClient())
                     {
                         client.BaseAddress = new Uri("http://win8dev");
                         client.DefaultRequestHeaders.Accept.Add(
                             new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                         var resp = client.PostAsJsonAsync("inTimePayment/api/paySOA/createBillHelpForTest", new createBillParaWarp{Para = new inTimeReqPara{ReqId=reqId},Method=pm,BankPara = new BankParaWarp{ProcessTime=pt,Result=rt}}).Result;
                         var resContent = resp.Content.ReadAsAsync<paymentEntity>();
                         //using (var req = new BankServiceClient())
                         //{
                         //    req.processPaymentHelp(be);
                         //}
                         ////using (var req = new PaymentServiceClient())
                         //{
                         //    req.paymentResultHookHelp(res);
                         //}
                         return resContent.Result;
                     }
                 }, testPara.WaitTime, testPara.CreatBillPara.Method, testPara.CreatBillPara.BankPara.ProcessTime, testPara.CreatBillPara.BankPara.Result);
        }
    }
}
