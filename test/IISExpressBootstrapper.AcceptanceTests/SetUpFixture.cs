using NUnit.Framework;
using System.Reflection;

namespace IISExpressBootstrapper.AcceptanceTests
{
    [SetUpFixture]
    public class SetUpFixture
    {
        public static string FileApplicationHostPath { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var binDir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath);
            var webDir = Path.GetFullPath(Path.Combine(binDir, "..", "..", "..", "..", "..", "sampleapp", "SampleApp"));
            if (!Directory.Exists(webDir))
                throw new Exception($"Directory does not exist at '{webDir}'.");
            FileApplicationHostPath = Path.Combine(binDir, "applicationhost.config");
            if (!File.Exists(FileApplicationHostPath))
                throw new Exception($"Application host config file does not exist at '{FileApplicationHostPath}'.");
            var contents = File.ReadAllText(FileApplicationHostPath);
            File.WriteAllText(FileApplicationHostPath, contents.Replace("__physicalPath__", webDir));
        }

    }
}
