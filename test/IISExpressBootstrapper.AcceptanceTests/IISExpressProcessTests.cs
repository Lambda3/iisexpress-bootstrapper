using FluentAssertions;
using NUnit.Framework;

namespace IISExpressBootstrapper.AcceptanceTests
{
    [TestFixture]
    public class IISExpressProcessTests
    {
        [Test]
        public void PreferX64Test()
        {
            if (!Environment.Is64BitProcess)
                return;
            if (Environment.GetEnvironmentVariable("ProgramW6432") == null)
                throw new Exception("Missing ProgramW6432 environment variable.");
            if (Environment.GetEnvironmentVariable("ProgramFiles(x86)") == null)
                throw new Exception("Missing ProgramFiles(x86) environment variable.");
            var configuration = new Configuration
            {
                ProcessParameters = new ConfigFileParameters(),
                PreferX64 = true
            };
            var iisExpressProcess = new IISExpressProcess(configuration);
            iisExpressProcess.IISExpressPath.Should().Be($@"{Environment.GetEnvironmentVariable("ProgramW6432")}\IIS Express\IISExpress.exe");
        }

        [Test]
        public void PreferX86Test()
        {
            if (!Environment.Is64BitProcess)
                return;
            if (Environment.GetEnvironmentVariable("ProgramW6432") == null)
                throw new Exception("Missing ProgramW6432 environment variable.");
            if (Environment.GetEnvironmentVariable("ProgramFiles(x86)") == null)
                throw new Exception("Missing ProgramFiles(x86) environment variable.");
            var configuration = new Configuration
            {
                ProcessParameters = new ConfigFileParameters(),
                PreferX64 = false
            };
            var iisExpressProcess = new IISExpressProcess(configuration);
            iisExpressProcess.IISExpressPath.Should().Be($@"{Environment.GetEnvironmentVariable("ProgramFiles(x86)")}\IIS Express\IISExpress.exe");
        }
    }
}
