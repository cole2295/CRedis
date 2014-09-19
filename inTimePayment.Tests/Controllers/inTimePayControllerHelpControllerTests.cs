using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inTimePayment.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ctripRedisHelp;
namespace inTimePayment.Controllers.Tests
{
    [TestClass()]
    public class inTimePayControllerHelpControllerTests
    {
        [TestMethod()]
        public void payRequestHelpTest()
        {
            var rId = "normal";
            var test = new inTimePayControllerHelpController();
            var res = test.payRequestHelp(rId);
            var assR = new inTimeRes { PaymentResult = "ok", PayMethod = payMethod.sync };
            Assert.AreEqual(res.PaymentResult, assR.PaymentResult);
            Assert.AreEqual(res.PayMethod, assR.PayMethod);
        }

        [TestMethod()]
        public void payRequestHelpForTestTest()
        {
            var rId = "Test";
            var bankPara = new BankParaWarp{ProcessTime=2000,Result="ok"};
            var creatBillPara = new createBillParaWarp{Method=payMethod.sync,BankPara=bankPara};
            var testPara = new testParameter{ReqId=rId,WaitTime=5000,CreatBillPara=creatBillPara};
            var test = new inTimePayControllerHelpController();
            var res = test.payRequestHelpForTest(rId, testPara);
            var assR = new inTimeRes { PaymentResult = "ok", PayMethod = payMethod.sync };
            Assert.AreEqual(res.PaymentResult, assR.PaymentResult);
            Assert.AreEqual(res.PayMethod, assR.PayMethod);
        }

        [TestMethod()]
        public void payRequestHelpForTestCase1Test()
        {
            var rId = "test1";
            var bankPara = new BankParaWarp { ProcessTime = 2000, Result = "failed" };
            var creatBillPara = new createBillParaWarp { Method = payMethod.sync, BankPara = bankPara };
            var testPara = new testParameter { ReqId = rId, WaitTime = 5000, CreatBillPara = creatBillPara };
            var test = new inTimePayControllerHelpController();
            var res = test.payRequestHelpForTest(rId, testPara);
            var assR = new inTimeRes { PaymentResult = "failed", PayMethod = payMethod.sync };
            Assert.AreEqual(res.PaymentResult, assR.PaymentResult);
            Assert.AreEqual(res.PayMethod, assR.PayMethod);
        }

        [TestMethod()]
        public void payRequestHelpForTestCase2Test()
        {
            var rId = "test2";
            var bankPara = new BankParaWarp { ProcessTime = 5500, Result = "failed" };
            var creatBillPara = new createBillParaWarp { Method = payMethod.sync, BankPara = bankPara };
            var testPara = new testParameter { ReqId = rId, WaitTime = 5000, CreatBillPara = creatBillPara };
            var test = new inTimePayControllerHelpController();
            var res = test.payRequestHelpForTest(rId, testPara);
            Assert.IsTrue(res == null);
        }

        [TestMethod()]
        public void payRequestHelpForTestCase3Test()
        {
            var rId = "test3";
            var bankPara = new BankParaWarp { ProcessTime = 2000, Result = string.Empty };
            var creatBillPara = new createBillParaWarp { Method = payMethod.async, BankPara = bankPara };
            var testPara = new testParameter { ReqId = rId, WaitTime = 5000, CreatBillPara = creatBillPara };
            var test = new inTimePayControllerHelpController();
            var res = test.payRequestHelpForTest(rId, testPara);
            var assR = new inTimeRes { PaymentResult = string.Empty, PayMethod = payMethod.async };
            Assert.AreEqual(res.PaymentResult, assR.PaymentResult);
            Assert.AreEqual(res.PayMethod, assR.PayMethod);
        }

    }
}
