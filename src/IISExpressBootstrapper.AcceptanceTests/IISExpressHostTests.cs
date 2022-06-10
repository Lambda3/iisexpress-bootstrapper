using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Net;

namespace IISExpressBootstrapper.AcceptanceTests
{
    [TestFixture]
    public class IISExpressHostTests
    {
        private IDictionary<string, string> environmentVariables;
        private IISExpressHost host;

        [OneTimeSetUp]
        public void SetUp()
        {
            environmentVariables = new Dictionary<string, string> { { "Foo1", "Bar1" }, { "Sample2", "It work's!" } };

            host = IISExpressHost.Start("IISExpressBootstrapper.SampleApp", 8088, environmentVariables);
        }

        [OneTimeTearDown]
        public void TearDown() => host?.Dispose();

        [Test]
        public async Task ShouldRunTheWebApplication()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:8088/");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task ShouldRunTheWebApiApplication()
        {
            using var client = new HttpClient();
            var response = await client.GetAsync("http://localhost:8088/api/sampleapi/10");
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be("\"You send me 10\"");
        }

        [Test]
        public async Task ShouldSetEnvironmentVariablesAsync()
        {
            await TestEnvironmentVariableAsync("Foo1", environmentVariables["Foo1"]);
            await TestEnvironmentVariableAsync("Sample2", environmentVariables["Sample2"]);
        }

        private static async Task TestEnvironmentVariableAsync(string variable, string expected)
        {
            const string url = "http://localhost:8088/Home/EnvironmentVariables?name={0}";
            using var client = new HttpClient();
            var response = await client.GetAsync(string.Format(url, variable));
            var content = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            content.Should().Be(expected);
        }

        [Test]
        public void ThrowExceptionWhenNotFoundIISExpressPath()
        {
            const string iisExpressPath = @"Z:\Foo\Bar\iis.exe";
            var action = () => IISExpressHost.Start(null, iisExpressPath: iisExpressPath);
            action.Should().Throw<IISExpressNotFoundException>();
        }

        [Test]
        public void ThrowExceptionWhenNotFoundWebApplicationPath()
        {
            var action = () => IISExpressHost.Start("Foo.Bar.Web", 8088);
            action.Should().Throw<DirectoryNotFoundException>()
                .WithMessage("Could not infer the web application folder path.");
        }

        [Test]
        public void StartingWithInvalidConfigurationShouldWriteMessage()
        {
            string s = null;
            IISExpressHost.Start(new ConfigFileParameters { ConfigFile = "", SiteName = "Does not exist" },
                output: message => { s = message; });
            s.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void IfProcessIsRunningShouldShowIt() => host.IsRunning.Should().BeTrue();

        [Test]
        public void IfProcessIsRunningShouldShowProcessId() => host.ProcessId.Should().BeGreaterThan(0);

        [Test]
        public void IfProcessIsRunningShouldShowInProcessesList()
        {
            using var process = Process.GetProcessById(host.ProcessId);
            process.Should().NotBeNull();
        }

        [Test]
        public void StartingAndKillingAProcessShouldShowHostNotToBeRunning()
        {
            var brokenHost = IISExpressHost.Start(new ConfigFileParameters { ConfigFile = "", SiteName = "Does not exist" });
            using var process = Process.GetProcessById(brokenHost.ProcessId);
            process.Kill();
            brokenHost.IsRunning.Should().BeFalse();
        }
    }
}
