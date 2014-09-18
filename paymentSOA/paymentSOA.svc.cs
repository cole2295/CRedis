using ctripRedisHelp;
using paymentSOA.bankService;
using ServiceStack.Text;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace paymentSOA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class paymentService : IPaymentService
    {
        public paymentEntity createBill(inTimeReqPara para)
        {
            var billNoTmp = para.ReqId + DateTime.Now.Ticks;
            var res = new paymentEntity { BillNo = billNoTmp };
            
            var bankEnt = new bankEntity { PayId = billNoTmp };
            var t = Task.Factory.StartNew(() =>
                {
                    using (var req = new BankServiceClient())
                    {
                        req.processPayment(bankEnt);
                    }
                }
                );

            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;

            try
            {
                using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
                {
                    var strTmp = JsonSerializer.SerializeToString(new { reqId = para.ReqId, billno = billNoTmp });
                    var ts = new TimeSpan(0, 5, 0);

                    help.set(bankEnt.PayId, strTmp, ts);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            t.Wait(100);
            return res;
        }


        public void paymentResultHook(inTimeEntity payResult)
        {
            var ip = "jimmyMStation";// "172.16.144.70";
            var port = 6379;

            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var strTmp = help.get(payResult.BankEnt.PayId);
                var payInfoTmp = strTmp.FromJson<Dictionary<string, string>>();
                //var subItem = payInfoTmp["billno"];
                //var waitTime = 5000;
                //var t = help.subscriberInTime(subItem, waitTime);

                var pubItem = payInfoTmp["reqId"];
                var pubValue = payResult.PaymentResult;

                var subNum = help.publish(pubItem, pubValue);

                //var methodRes = t.Result;
                if (subNum > 0)
                {
                    //go selfMehod
                }
                else
                {
                    //go async
                }
                //var syncStr = "sync";
                //if (subNum > 0)
                //{
                //    //go sync
                //}
                //else
                //{
                //    //go async
                //}


            }
        }

        public paymentEntity createBillHelp(inTimeReqPara para)
        {
            var billNoTmp = para.ReqId + DateTime.Now.Ticks;
            var res = new paymentEntity { BillNo = billNoTmp };
            
            var bankEnt = new bankEntity { PayId = billNoTmp };
            var t = Task.Factory.StartNew(() =>
            {
                using (var req = new BankServiceClient())
                {
                    req.processPaymentHelp(bankEnt);
                }
            });

            var ip = "jimmyMStation";//"172.16.144.70";
            var port = 6379;
            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var ts = new TimeSpan(0, 5, 0);

                help.set(bankEnt.PayId, para.ReqId, ts);
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
                var subItem = help.get(payResult.BankEnt.PayId);;
                //var waitTime = 5000;
                //var res = help.inTimePaymentResultHelp(subItem, payResult.PaymentResult, waitTime, () =>
                //{
                //    return true;
                //});
                var res = help.inTimePaymentResultHelp(subItem, payResult.PaymentResult, payMethod.sync);

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
