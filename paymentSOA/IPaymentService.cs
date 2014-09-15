using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace paymentSOA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        paymentEntity createBill(inTimeReqPara para);

        [OperationContract]
        void paymentResultHook(inTimeEntity payResult);
        [OperationContract]
        paymentEntity createBillHelp(inTimeReqPara para);

        [OperationContract]
        void paymentResultHookHelp(inTimeEntity payResult);

        // TODO: Add your service operations here
    }
}
