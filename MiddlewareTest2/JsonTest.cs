using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;

namespace Tests
{
    public class JsonDynamicParseTest
    {
        private string JSON = string.Empty;
        [SetUp]
        public void Setup()
        {
            using (StreamReader r = new StreamReader(@"C:\Users\JWK8HKX\source\repos\CustomHeaderMiddleware\MiddlewareTest2\parse.json"))
            {
                JSON = r.ReadToEnd();
            }
        }

        [Test]
        public void FirstNameTest()
        {
            if (!string.IsNullOrEmpty(JSON))
            {
                dynamic stuff = JObject.Parse(JSON);

                var data = stuff.result[0];
                Console.WriteLine("FirstName : " + data.firstName);
                Assert.AreEqual("Recent22", data.firstName);
            }
            Assert.Pass();
        }
    }
}