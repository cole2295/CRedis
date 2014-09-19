using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using ServiceStack.Text;
using ServiceStack;
using System.Threading.Tasks;

namespace inTimePayServer.Controllers
{
    public class paySOAController : ApiController
    {
        [DataContract]
        public class inTimeReqPara
        {
            string reqId = string.Empty;

            [DataMember]
            public string ReqId
            {
                get { return reqId; }
                set { reqId = value; }
            }
        }

        [DataContract]
        public class inTimeEntity
        {
            string paymentResult = string.Empty;

            [DataMember]
            public string PaymentResult
            {
                get { return paymentResult; }
                set { paymentResult = value; }
            }


            bankController.bankEntity bankEnt;

            [DataMember]
            public bankController.bankEntity BankEnt
            {
                get { return bankEnt; }
                set { bankEnt = value; }
            }

        }


        [DataContract]
        public class paymentEntity
        {
            string billNo = string.Empty;

            [DataMember]
            public string BillNo
            {
                get { return billNo; }
                set { billNo = value; }
            }
        }

        public paymentEntity createBillHelpForTest(inTimeReqPara para, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            //return realCreateBill(para, (be, pt, rt) =>
            //{
            //    HttpClient client = new HttpClient();
            //    //client.BaseAddress = new Uri("http://localhost/inTimePayServer");
            //    client.DefaultRequestHeaders.Accept.Add(
            //        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //    var res = client.GetAsync("http://localhost/inTimePayServer/api/processPaymentHelpForTest?reqId=test").Result;
            //    var resContent = res.Content.ReadAsAsync<string>();
            //    using (var req = new BankServiceClient())
            //    {
            //        req.processPaymentHelp(be);
            //    }
            //}, method, processTime, result);
            return new paymentEntity();
        }

        public paymentEntity createBillHelp(inTimeReqPara para)
        {
            return realCreateBill(para, (be, pt, rt) =>
            {
                //using (var req = new BankServiceClient())
                //{
                //    req.processPaymentHelpForTest(be, pt, rt);
                //}
            });
        }

        public paymentEntity realCreateBill(inTimeReqPara para, Action<inTimePayServer.Controllers.bankController.bankEntity, int, string> req, payMethod method = payMethod.sync, int processTime = 2000, string result = "ok")
        {
            var billNoTmp = para.ReqId + DateTime.Now.Ticks;
            var res = new paymentEntity { BillNo = billNoTmp };

            var bankEnt = new inTimePayServer.Controllers.bankController.bankEntity { PayId = billNoTmp };
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
                Enum.TryParse<payMethod>(payInfoTmp["method"], out method);
                //var subItem = help.get(payResult.BankEnt.PayId);
                //var waitTime = 5000;
                //var res = help.inTimePaymentResultHelp(subItem, payResult.PaymentResult, waitTime, () =>
                //{
                //    return true;
                //});
                var res = help.inTimePaymentResultHelp(subItem, payResult.PaymentResult, method);

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
