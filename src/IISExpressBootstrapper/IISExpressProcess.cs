using System;
using System.IO;

namespace IISExpressBootstrapper
{
    internal class IISExpressProcess : IDisposable
    {
        private readonly ProcessRunner process;

        public IISExpressProcess(Configuration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.IISExpressPath))
            {
                configuration.IISExpressPath = GetDefaultIISExpressPath();
            }
            if (!File.Exists(configuration.IISExpressPath))
            {
                throw new IISExpressNotFoundException();
            }

            process = ProcessRunner.Run(configuration.IISExpressPath, configuration.ProcessParameters.ToString(), configuration.EnvironmentVariables);
        }

        public void Dispose()
        {
            process.Dispose();
        }

        private static string GetDefaultIISExpressPath()
        {
            const string iisExpressX86Path = @"c:\program files (x86)\IIS Express\IISExpress.exe";
            const string iisExpressX64Path = @"c:\program files\IIS Express\IISExpress.exe";

            return File.Exists(iisExpressX86Path) ? iisExpressX86Path : iisExpressX64Path;
        }
    }
}
