using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace bankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBankService
    {
        [OperationContract]
        void processPayment(bankEntity payEntity);
        [OperationContract]
        void processPaymentHelp(bankEntity payEntity);

        // TODO: Add your service operations here
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
}
