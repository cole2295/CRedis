using ctripRedisHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace inTimePayment.Controllers
{
    [DataContract]
    public class testParameter
    {
        string reqId = string.Empty;
        [DataMember]
        public string ReqId
        {
            get { return reqId; }
            set { reqId = value; }
        }
        int waitTime = 5000;
        [DataMember]
        public int WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }

        private createBillParaWarp creatBillPara = new createBillParaWarp();
        [DataMember]
        public createBillParaWarp CreatBillPara
        {
            get { return creatBillPara; }
            set { creatBillPara = value; }
        }
    }

    [DataContract]
    public class createBillParaWarp
    {
        inTimeReqPara para = new inTimeReqPara();
        [DataMember]
        public inTimeReqPara Para
        {
            get { return para; }
            set { para = value; }
        }

        payMethod method = payMethod.sync;
        [DataMember]
        public payMethod Method
        {
            get { return method; }
            set { method = value; }
        }

        BankParaWarp bankPara = new BankParaWarp();
        [DataMember]
        public BankParaWarp BankPara
        {
            get { return bankPara; }
            set { bankPara = value; }
        }
    }

    [DataContract]
    public class BankParaWarp
    {

        bankEntity bankEentity = new bankEntity();
        [DataMember]
        public bankEntity BankEentity
        {
            get { return bankEentity; }
            set { bankEentity = value; }
        }
        int processTime = 2000;
        [DataMember]
        public int ProcessTime
        {
            get { return processTime; }
            set { processTime = value; }
        }
        string result = "ok";
        [DataMember]
        public string Result
        {
            get { return result; }
            set { result = value; }
        }
    }

    [DataContract]
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


        bankEntity bankEnt;

        [DataMember]
        public bankEntity BankEnt
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

    [DataContract]
    public class inTimeRes
    {
        string paymentResult = string.Empty;
        [DataMember]
        public string PaymentResult
        {
            get { return paymentResult; }
            set { paymentResult = value; }
        }
        payMethod payMethod = payMethod.async;
        [DataMember]
        public payMethod PayMethod
        {
            get { return payMethod; }
            set { payMethod = value; }
        }
    }
}