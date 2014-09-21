using System;
namespace inTimePayment.Controllers
{
    public interface IpaySOAController
    {
        paymentEntity createBillHelp(inTimeReqPara para);
        paymentEntity createBillHelpForTest(createBillParaWarp createBillPara);
        void paymentResultHookHelp(inTimeEntity payResult);
    }
}
