using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace inTimePayment.Controllers
{
    public class bank
    {
        public void realProcessPaymentHelp(bankEntity bankEntity, int processTime = 2000, string result = "ok")
        {
            Thread.Sleep(processTime);
            var res = new inTimeEntity { PaymentResult = result, BankEnt = new bankEntity { PayId = bankEntity.PayId } };
            var t = Task.Factory.StartNew(() =>
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://win8dev");
                    client.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var resp = client.PostAsJsonAsync("inTimePayment/api/paySOA/paymentResultHookHelp", res).Result;
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

            t.Wait(100);

            //Thread.Sleep(3000);
            //    var res = new inTimeEntity();
            //    res.PaymentResult = "ok";
        }
    }
    [RoutePrefix("api/bank")]
    public class bankController : ApiController
    {
        [Route("processPaymentHelpForTest")]
        [HttpPost]
        public void processPaymentHelpForTest(BankParaWarp bankPara)
        {
            new bank().realProcessPaymentHelp(bankPara.BankEentity, bankPara.ProcessTime, bankPara.Result);
        }

        [Route("processPaymentHelp")]
        [HttpPost]
        public void processPaymentHelp(bankEntity bankEntity)
        {
            new bank().realProcessPaymentHelp(bankEntity);
        }
    }
}
