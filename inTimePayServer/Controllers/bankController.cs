using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace inTimePayServer.Controllers
{
    public class bankController : ApiController
    {
        public class bankEntity
        {
            string payId = string.Empty;

            [DataMember]
            public string PayId
            {
                get { return payId; }
                set { payId = value; }
            }
        }

        public void processPaymentHelpForTest(bankEntity bankEntity, int processTime = 2000, string result = "ok")
        {
            Thread.Sleep(processTime);
            var res = new inTimePayServer.Controllers.paySOAController.inTimeEntity { PaymentResult = result, BankEnt = new bankEntity { PayId = bankEntity.PayId } };
            var t = Task.Factory.StartNew(() =>
            {
                //using (var req = new PaymentServiceClient())
                //{
                //    req.paymentResultHookHelp(res);
                //}

            });

            t.Wait(100);

            //Thread.Sleep(3000);
            //    var res = new inTimeEntity();
            //    res.PaymentResult = "ok";
        }


        public void processPaymentHelp(bankEntity bankEntity)
        {
            processPaymentHelpForTest(bankEntity);
        }
    }
}
