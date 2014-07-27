using FluentAssertions;
using NUnit.Framework;
using System.Net;

namespace IISExpressBootstrapper.AcceptanceTests
{
    [TestFixture]
    public class IISExpressHostTests
    {
        private IISExpressHost host;
        
        [SetUp]
        public void SetUp()
        {
            host = new IISExpressHost("IISExpressBootstrapper.SampleWebApp", 8088);
        }

        [Test]
        public void ShouldRunTheWebApplication()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://localhost:8088/");

            var response = (HttpWebResponse)request.GetResponse();

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            host.Dispose();
        }
    }
}
