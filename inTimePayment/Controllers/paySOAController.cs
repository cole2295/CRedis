using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceStack;
using ServiceStack.Text;
using System.Threading.Tasks;
using inTimePaymentForSoa;

namespace inTimePayment.Controllers
{
    public class paySOA
    {
        public paymentEntity realCreateBill(inTimeReqPara para, Action<bankEntity, int, string> req, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            var billNoTmp = para.ReqId + DateTime.Now.Ticks;
            var res = new paymentEntity { BillNo = billNoTmp };

            var bankEnt = new bankEntity { PayId = billNoTmp };
            var t = Task.Factory.StartNew(() =>
            {
                req(bankEnt, processTime, result);
            });

            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;
            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var setItem = JsonSerializer.SerializeToString(new { reqId = para.ReqId, payMethod = method });
                var ts = new TimeSpan(0, 5, 0);

                help.set(bankEnt.PayId, setItem, ts);
                //help.set(bankEnt.PayId, para.ReqId, ts);
            }

            t.Wait(100);
            return res;
        }
    }

    [RoutePrefix("api/paySOA")]
    public class paySOAController : ApiController
    {
        [Route("createBillHelpForTest")]
        [HttpPost]
        public paymentEntity createBillHelpForTest(createBillParaWarp createBillPara)//inTimeReqPara para, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            return new paySOA().realCreateBill(createBillPara.Para, (be, pt, rt) =>
            {
                var forBank = new BankParaWarp { BankEentity = be, ProcessTime = pt, Result = rt };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://win8dev");
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var resp = client.PostAsJsonAsync("inTimePayment/api/bank/processPaymentHelpForTest", forBank).Result;
                    var resContent = resp.Content.ReadAsAsync<string>();
                    //using (var req = new BankServiceClient())
                    //{
                    //    req.processPaymentHelp(be);
                    //}
                    ////using (var req = new PaymentServiceClient())
                    //{
                    //    req.paymentResultHookHelp(res);
                    //}
                }
            }, createBillPara.Method, createBillPara.BankPara.ProcessTime, createBillPara.BankPara.Result);
        }

        [Route("createBillHelp")]
        [HttpPost]
        public paymentEntity createBillHelp(inTimeReqPara para)
        {
            return new paySOA().realCreateBill(para, (be, pt, rt) =>
            {
                var forBank = be;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://win8dev");
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var resp = client.PostAsJsonAsync("inTimePayment/api/bank/processPaymentHelp", forBank).Result;
                    var resContent = resp.Content.ReadAsAsync<string>();
                    //using (var req = new BankServiceClient())
                    //{
                    //    req.processPaymentHelp(be);
                    //}
                    ////using (var req = new PaymentServiceClient())
                    //{
                    //    req.paymentResultHookHelp(res);
                    //}
                }
            });
        }


        [Route("paymentResultHookHelp")]
        [HttpPost]
        public void paymentResultHookHelp(inTimeEntity payResult)
        {
            var ip = "jimmyMStation";// "172.16.144.70";
            var port = 6379;

            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var strTmp = help.get(payResult.BankEnt.PayId);
                var payInfoTmp = strTmp.FromJson<Dictionary<string, string>>();
                var subItem = payInfoTmp["reqId"];
                payMethod method;
                Enum.TryParse<payMethod>(payInfoTmp["payMethod"], out method);
                //var subItem = help.get(payResult.BankEnt.PayId);
                //var waitTime = 5000;
                //var res = help.inTimePaymentResultHelp(subItem, payResult.PaymentResult, waitTime, () =>
                //{
                //    return true;
                //});
                var resToPub = new inTimeRes { PaymentResult = payResult.PaymentResult, PayMethod = method };

                var pubItem = resToPub.ToJson();

                var res = help.inTimePaymentResultHelp(subItem, pubItem, method);

                //return res;

                var methodRes = res;
                //var syncStr = "sync";
                if (payMethod.sync.Equals(methodRes))
                {
                    //go sync
                }
                else
                {
                    //go async
                }


            }
        }
    }
}
