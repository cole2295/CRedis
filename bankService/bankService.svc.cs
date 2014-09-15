using bankService.paymentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IBankService
    {
        public void processPayment(bankEntity bankEntity)
        {
            var req = new PaymentServiceClient();
            Thread.Sleep(2000);
            var res = new inTimeEntity { PaymentResult = "ok", BankEnt = new paymentService.bankEntity { PayId = bankEntity.PayId } };
            var t = Task.Factory.StartNew(() =>
            {
                req.paymentResultHook(res);

            });

            t.Wait(100);
                
            //Thread.Sleep(3000);
            //    var res = new inTimeEntity();
            //    res.PaymentResult = "ok";
        }

        public void processPaymentHelp(bankEntity bankEntity)
        {
            var req = new PaymentServiceClient();
            Thread.Sleep(2000);
            var res = new inTimeEntity { PaymentResult = "ok", BankEnt = new paymentService.bankEntity { PayId = bankEntity.PayId } };
            var t = Task.Factory.StartNew(() =>
            {
                req.paymentResultHookHelp(res);

            });

            t.Wait(100);

            //Thread.Sleep(3000);
            //    var res = new inTimeEntity();
            //    res.PaymentResult = "ok";
        }
    }
}
