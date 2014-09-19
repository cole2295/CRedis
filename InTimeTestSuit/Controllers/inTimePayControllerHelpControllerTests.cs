using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inTimePayServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
namespace inTimePayServer.Controllers.Tests
{
    [TestClass()]
    public class inTimePayControllerHelpControllerTests
    {

        public TestContext TestContext { get; set; }

        [ClassInitialize()]
        public static void Init(TestContext context)
        {
            //var resContent = res.Content.ReadAsAsync()
            Console.WriteLine("Use ClassInitialize to run code before you run the first test in the class.");
        }

        //[TestMethod()]
        //public void payRequestHelpTest()
        //{
        //    HttpClient client = new HttpClient();
        //    //client.BaseAddress = new Uri("http://localhost/inTimePayServer");
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //    var res = client.GetAsync("http://localhost/inTimePayServer/api/inTimePayControllerHelp?reqId=test").Result;
        //    var resContent = res.Content.ReadAsAsync<string>();
        //    Assert.Fail();
        //}
    }
}
