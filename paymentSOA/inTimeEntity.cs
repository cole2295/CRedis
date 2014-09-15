using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace paymentSOA
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

        bankService.bankEntity bankEnt;

        [DataMember]
        public bankService.bankEntity BankEnt
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
}
