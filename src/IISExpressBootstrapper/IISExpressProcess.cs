using System;
using System.IO;

namespace IISExpressBootstrapper
{
    internal sealed class IISExpressProcess : IDisposable
    {
        private readonly ProcessRunner process;

        public bool IsRunning => process.IsRunning;

        public int ProcessId => process.ProcessId;

        public IISExpressProcess(Configuration configuration)
        {
            if (string.IsNullOrWhiteSpace(configuration.IISExpressPath))
                configuration.IISExpressPath = GetDefaultIISExpressPath();
            if (!File.Exists(configuration.IISExpressPath))
                throw new IISExpressNotFoundException();

            process = ProcessRunner.Run(configuration.IISExpressPath, configuration.ProcessParameters.ToString(), configuration.EnvironmentVariables,
                configuration.Output);
        }

        public void Dispose() => process.Dispose();

        private static string GetDefaultIISExpressPath()
        {
            var iisExpressPath = $@"{Environment.GetEnvironmentVariable("ProgramFiles")}\IIS Express\IISExpress.exe";
            var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (programFilesX86 == null)
                return iisExpressPath;
            var iisExpressX86Path = $@"{programFilesX86}\IIS Express\IISExpress.exe";
            return File.Exists(iisExpressX86Path) ? iisExpressX86Path : iisExpressPath;
        }
    }
}
