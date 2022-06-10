using FluentAssertions;
using NUnit.Framework;

namespace IISExpressBootstrapper.AcceptanceTests
{
    [TestFixture]
    public class ParametersTests
    {
        [Test]
        public void DefaultConfigFileParametersShouldBeEmpty() =>
            new ConfigFileParameters().ToString().Should().Be(string.Empty);

        [Test]
        public void DefaultPathParametersShouldBeEmpty() =>
            new PathParameters().ToString().Should().Be(string.Empty);

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SetParameterSystray(bool value)
        {
            var configFileParameters = new ConfigFileParameters { Systray = value };
            var pathParameters = new PathParameters { Systray = value };
            var expected = " /systray:" + value.ToString().ToLower();
            configFileParameters.ToString().Should().Be(expected);
            pathParameters.ToString().Should().Be(expected);
        }

        [Test]
        [TestCase(TraceLevel.Error)]
        [TestCase(TraceLevel.Info)]
        [TestCase(TraceLevel.Warning)]
        public void SetParameterTraceLevel(TraceLevel traceLevel)
        {
            var configFileParameters = new ConfigFileParameters { TraceLevel = traceLevel };
            var pathParameters = new PathParameters { TraceLevel = traceLevel };
            var expected = " /trace:" + traceLevel.ToString().ToLower();
            configFileParameters.ToString().Should().Be(expected);
            pathParameters.ToString().Should().Be(expected);
        }

        [Test]
        public void SetParameterConfigFile()
        {
            const string value = @"C:\MyPath\applicationhosts.config";
            var parameters = new ConfigFileParameters { ConfigFile = value };
            parameters.ToString().Should().Be($@" /config:""{value}""");
        }

        [Test]
        public void SetParameterSiteId()
        {
            const string value = @"Foo";
            var parameters = new ConfigFileParameters { SiteId = value };
            parameters.ToString().Should().Be($@" /siteid:""{value}""");
        }

        [Test]
        public void SetParameterSiteName()
        {
            const string value = @"My Site Name";
            var parameters = new ConfigFileParameters { SiteName = value };
            parameters.ToString().Should().Be($@" /site:""{value}""");
        }

        [Test]
        public void SetParameterClrVersion()
        {
            const string value = @"v4.0";
            var parameters = new PathParameters { ClrVersion = value };
            parameters.ToString().Should().Be($@" /clr:""{value}""");
        }

        [Test]
        public void SetParameterPath()
        {
            const string value = @"C:\My Path\MySiteFolder";
            var parameters = new PathParameters { Path = value };
            parameters.ToString().Should().Be($@" /path:""{value}""");
        }

        [Test]
        public void SetParameterPort()
        {
            const int value = 8088;
            var parameters = new PathParameters { Port = value };
            parameters.ToString().Should().Be($@" /port:{value}");
        }
    }
}
