using System;
namespace inTimePayment.Controllers
{
    public interface IbankController
    {
        void processPaymentHelp(bankEntity bankEntity);
        void processPaymentHelpForTest(BankParaWarp bankPara);
    }
}
