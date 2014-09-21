using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inTimePaymentForService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ctripRedisHelp;
using ServiceStack;
using ServiceStack.Text;
using Moq;
using System.Linq.Expressions;
using StackExchange.Redis;

namespace inTimePaymentForService.Tests
{
    [TestClass()]
    public class serviceHelpTests
    {
        [TestMethod()]
        public void inTimePaymentMethodHelpTest()
        {
            var subKey = "inTimePaymentMethodHelpTest";
            var pubKey = "inTimePaymentMethodHelpTestPub";
            var timeOut = 1000;
            var mockHelp = new Mock<IhelpBase>();
            //mockHelp.Setup(vp => vp.valueProcessor(pubKey)).Callback(() =>
            //{
            //    mockHelp.Setup(iv => iv.InValue).Returns(pubKey);
            //});
            mockHelp.Setup(sub => sub.subscriber(subKey)).Callback(() =>
            {
                mockHelp.Setup(iv => iv.InValue).Returns(pubKey);
            });

            mockHelp.Setup(wait => wait.Wait(100)).Returns(true);

            var helpPara = mockHelp.Object;
            var res = helpPara.inTimePaymentMethodHelp(subKey, timeOut, () => true);

            Assert.AreEqual(res, pubKey);
            res = helpPara.inTimePaymentMethodHelp(subKey, timeOut, () => false);
            Assert.AreEqual(res,string.Empty);
        }
    }
}
