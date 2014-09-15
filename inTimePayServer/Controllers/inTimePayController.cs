using ctripRedisHelp;
using inTimePayServer.paySOAService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace inTimePayServer.Controllers
{
    public class inTimePayController : ApiController
    {
        //[Route("payrequest")]
        [HttpGet]
        public string payRequest(string reqId)
        {
            var ip = "jimmyMStation.local";//"172.16.144.70";
            var port = 6379;

            using (var help = helpBase.init(ip, port, new TimeSpan(0, 5, 0)))
            {
                var subitem = reqId;
                var waittime = 5000;
                var t = help.subscriberInTime(subitem, waittime);

                //request paymentsoa return billno
                var billres = new paymentEntity();
                using (var req = new PaymentServiceClient())
                {
                    billres = req.createBill(new inTimeReqPara { ReqId = reqId });
                }

                if (string.IsNullOrEmpty(billres.BillNo))
                {
                    return "failed";
                }
                var res = t.Result;

                var pubitem = billres.BillNo;
                var pubvalue = string.Empty;
                //thread.sleep(30000);    
                if (string.IsNullOrEmpty(res))
                {
                    pubvalue = "async";
                }
                else
                {
                    pubvalue = "sync";
                }

                help.publish(pubitem, pubvalue);


                return res;
            }
        }

    }
}
