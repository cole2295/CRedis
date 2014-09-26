using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ctripRedisHelp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System.Threading;
namespace ctripRedisHelp.Tests
{
    [TestClass()]
    public class helpBaseTests
    {
        static helpBase testHelp = null; 
        static string ip = string.Empty;//"172.16.144.70";
        static int port = 0;
        static TimeSpan ts = new TimeSpan(0, 0, 1);
        
        [ClassInitialize]
        static public void init(TestContext tc)
        {
            ip = "jimmyMStation";//"172.16.144.70";
            port = 6379;
            ts = new TimeSpan(0, 0, 5);

            testHelp = helpBase.init(ip, port, ts);
        }

        [ClassCleanup]
        static public void cleanUp()
        {
            if (testHelp.IsConnected)
            {
                testHelp.Dispose();
            }

            Assert.IsFalse(testHelp.IsConnected);
        }

        [TestMethod()]
        public void getSubscriberTest()
        {
            var help = helpBase.init(ip, port, ts);
            var sub = help.getSubscriber();

            Assert.IsTrue(sub is ISubscriber);
        }

        [TestMethod()]
        public void initTest()
        {
            var help = helpBase.init(ip, port, ts);
            if (help == null)
            {
                help = helpBase.init(ip, port, ts);
            }
            Assert.IsTrue(help is helpBase);
            var testKey = "initTest";
            var testItem = "initTestValue";
            help.publishAndSet(testKey, testItem);
            var resItem = help.get(testKey);
            Assert.AreEqual(resItem, testItem);
            Thread.Sleep(ts);
            resItem = help.get(testKey);
            Assert.AreNotEqual(resItem, testItem);
        }

        [TestMethod()]
        public void setTest()
        {
            var tsetTestTs = new TimeSpan(0, 0, 5);
            var help = helpBase.init(ip, port, tsetTestTs);
            var testKey = "setTest";
            var testItem = "setTestValue";
            var flag = help.set(testKey, testItem, tsetTestTs);
            Assert.IsTrue(flag);
            var resItem = help.get(testKey);
            Assert.AreEqual(resItem, testItem);
            Thread.Sleep(tsetTestTs);
            resItem = help.get(testKey);
            Assert.AreNotEqual(resItem, testItem);
        }

        [TestMethod()]
        public void setnxTest()
        {
            var setnxTestTs = new TimeSpan(0, 0, 5);
            var help = helpBase.init(ip, port, setnxTestTs);
            var testKey = "setnxTest";
            var testItem = "setnxTestValue";
            var flag = help.setnx(testKey, testItem, setnxTestTs);
            Assert.IsTrue(flag);
            flag = help.setnx(testKey, testItem, setnxTestTs);
            Assert.IsFalse(flag);
            var resItem = help.get(testKey);
            Assert.AreEqual(resItem, testItem);
            Thread.Sleep(setnxTestTs);
            resItem = help.get(testKey);
            Assert.AreNotEqual(resItem, testItem);
        }

        [TestMethod()]
        public void getTest()
        {
            var getTestTs = new TimeSpan(0, 0, 5);
            var help = helpBase.init(ip, port, getTestTs);
            var testKey = "getTest";
            var testItem = "getTestValue";
            var flag = help.set(testKey, testItem, getTestTs);
            Assert.IsTrue(flag);
            var resItem = help.get(testKey);
            Assert.AreEqual(resItem, testItem);
            help.set(testKey, testItem, new TimeSpan(0,0,0,0,1));
        }

        [TestMethod()]
        public void publishAndSetTest()
        {
            var help = helpBase.init(ip, port, ts);
            var testKey = "publishAndSetTest";
            var testItem = "publishAndSetTestValue";
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var res = string.Empty;
            help.subscriber(testKey);
            
            long num = help.publishAndSet(testKey, testItem);
            autoEvent.WaitOne(1000);

            res = help.InValue;
            help.unSubscriber(testKey);
            Assert.AreEqual(res, testItem);
            Assert.AreEqual(num, 1); 
            var resItem = help.get(testKey);
            Assert.AreEqual(resItem, testItem);
            Thread.Sleep(ts);
            resItem = help.get(testKey);
            Assert.AreNotEqual(resItem, testItem);
        }

        [TestMethod()]
        public void publishTest()
        {
            var help = helpBase.init(ip, port, ts);
            var testKey = "publishTest";
            var testItem = "publishTestValue";
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var res = string.Empty;
            help.subscriber(testKey);

            long num = help.publish(testKey, testItem);
            autoEvent.WaitOne(1000);

            res = help.InValue;
            help.unSubscriber(testKey);
            Assert.AreEqual(res, testItem);
            Assert.AreEqual(num, 1);
            var resItem = help.get(testKey);
            Assert.AreNotEqual(resItem, testItem);
        }

        //[TestCleanup]
        [TestMethod()]
        public void DisposeTest()
        {
            //help.Dispose();
            
            //Assert.IsFalse(help.IsConnected);

        }

        [TestMethod()]
        public void subscriberTest()
        {
            var help = helpBase.init(ip, port, ts);

            var testKey = "subscriberTest";
            var testItem = "subscriberTestValue";
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var res = string.Empty;
            help.subscriber(testKey);

            help.publish(testKey, testItem);
            autoEvent.WaitOne(1000);

            res = help.InValue;
            help.unSubscriber(testKey);
            Assert.AreEqual(res, testItem);
        }

        [TestMethod()]
        public void unSubscriberTest()
        {
            var help = helpBase.init(ip, port, ts);

            var testKey = "unSubscriberTest";
            var testItem = "unSubscriberTestValue";
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            var res = string.Empty;
            help.subscriber(testKey);
            help.unSubscriber(testKey);
            var num = help.publish(testKey, testItem);
            autoEvent.WaitOne(1000);

            res = help.InValue;
            Assert.AreNotEqual(res, testItem);
            
        }

        [TestMethod()]
        public void valueProcessorTest()
        {
            var help = helpBase.init(ip, port, ts);
            var testValue = "valueProcessorTestValue";
            var onValue = string.Empty;
            var timeOut = 1000;
            var sleepT = 500;
            help.onValueArrive += (sender, e) =>
            {
                onValue = e.Value;
            };

            var t1 = DateTime.Now.Ticks;
            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(sleepT);
                help.valueProcessor(testValue);
            });
            help.Wait(timeOut);
            var t2 = DateTime.Now.Ticks;
            var resT = t2 - t1;
            var assT1 = 10000 * sleepT;
            var assT2 = 10000 * (sleepT + 100);
            Assert.IsTrue(resT > assT1 && resT < assT2);

            //help.valueProcessor(testValue);
            Assert.AreEqual(help.InValue, testValue);
            Assert.AreEqual(onValue, testValue);

        }

        [TestMethod()]
        public void WaitTest()
        {
            var help = helpBase.init(ip, port, ts);
            var t1 = DateTime.Now.Ticks;
            var timeOut = 1000;
            help.Wait(timeOut);
            var t2 = DateTime.Now.Ticks;
            var resT = t2 - t1;
            var assT1 = 10000*timeOut;
            var assT2 = 10000*(timeOut + 100);
            Assert.IsTrue(resT > assT1 && resT < assT2);

            t1 = DateTime.Now.Ticks;
            var sleepT = 500;
            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(sleepT);
                help.valueProcessor("WaitTest");
            });
            help.Wait(timeOut);
            t2 = DateTime.Now.Ticks;
            resT = t2 - t1;
            assT1 = 10000 * sleepT;
            assT2 = 10000 * (sleepT + 100);
            Assert.IsTrue(resT > assT1 && resT < assT2);

        }
    }
}
