using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inTimePaymentForSoa;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ctripRedisHelp;
using Moq;
namespace inTimePaymentForSoa.Tests
{
    [TestClass()]
    public class soaHelpTests
    {
        [TestMethod()]
        public void inTimePaymentResultHelpTest()
        {
            var pubKey = "inTimePaymentResultHelpTest";
            var pubValue = "inTimePaymentResultHelpTestPub";
            var method = payMethod.sync;
            var mockHelp = new Mock<IhelpBase>();
            mockHelp.Setup(set => set.setnx(pubKey, pubValue, new TimeSpan(0,1,0))).Returns(true);

            var helpPara = mockHelp.Object;
            var res = helpPara.inTimePaymentResultHelpWithSetnx(pubKey, pubValue, method);

            Assert.AreEqual(res, method);
            mockHelp.Setup(set => set.setnx(pubKey, pubValue, new TimeSpan(0, 1, 0))).Returns(false);
            helpPara = mockHelp.Object;
            res = helpPara.inTimePaymentResultHelpWithSetnx(pubKey, pubValue, method);
            Assert.AreNotEqual(res, method);
        }
    }
}
