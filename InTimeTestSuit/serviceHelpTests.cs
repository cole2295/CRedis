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
            var pubValue = "inTimePaymentMethodHelpTestPub";
            var timeOut = 1000;
            var mockHelp = new Mock<IhelpBase>();
            //mockHelp.Setup(vp => vp.valueProcessor(pubKey)).Callback(() =>
            //{
            //    mockHelp.Setup(iv => iv.InValue).Returns(pubKey);
            //});
            mockHelp.Setup(sub => sub.subscriber(subKey)).Callback(() =>
            {
                mockHelp.Setup(iv => iv.InValue).Returns(pubValue);
            });

            mockHelp.Setup(get => get.get(subKey)).Returns(string.Empty);

            mockHelp.Setup(wait => wait.Wait(timeOut)).Returns(true);

            mockHelp.Setup(setnx => setnx.setnx(subKey, payMethod.async.ToString(), new TimeSpan(0, 1, 0))).Returns(false);//.get(subKey)).Returns(pubValue);

            var helpPara = mockHelp.Object;
            var res = helpPara.inTimePaymentMethodHelpWithSetnx(subKey, timeOut);
            //first get failed,but sub return the value
            Assert.AreEqual(res, pubValue);


            mockHelp.Setup(get => get.get(subKey)).Returns(pubValue);
            mockHelp.Setup(wait => wait.Wait(0)).Returns(true);
            res = helpPara.inTimePaymentMethodHelpWithSetnx(subKey, timeOut);
            //first get success
            Assert.AreEqual(res, pubValue);

            mockHelp.Setup(sub => sub.subscriber(subKey)).Callback(() =>
            {
                mockHelp.Setup(iv => iv.InValue).Returns(string.Empty);
            });
            mockHelp.Setup(wait => wait.Wait(0)).Returns(true);
            res = helpPara.inTimePaymentMethodHelpWithSetnx(subKey, timeOut);
            //first get failed,and sub is timeout,but setnx failed and got the result
            Assert.AreEqual(res, pubValue);

            mockHelp.Setup(sub => sub.subscriber(subKey)).Callback(() =>
            {
                mockHelp.Setup(iv => iv.InValue).Returns(string.Empty);
            });
            mockHelp.Setup(wait => wait.Wait(0)).Returns(true);
            mockHelp.Setup(setnx => setnx.setnx(subKey, payMethod.async.ToString(), new TimeSpan(0, 1, 0))).Returns(true);//.get(subKey)).Returns(pubValue);

            res = helpPara.inTimePaymentMethodHelpWithSetnx(subKey, timeOut);
            //first get failed,adn sub is timeout,and setnx success,so return string.empty
            Assert.AreEqual(res, string.Empty);
            
        }
    }
}
