using System;
using System.Diagnostics;

namespace IISExpressBootstrapper
{
    internal class ProcessRunner : IDisposable
    {
        private readonly ProcessStartInfo processStartInfo;
        private Process process;

        public static ProcessRunner Run(string exePath, string arguments = "")
        {
            var processRunner = new ProcessRunner(exePath, arguments);
            processRunner.Start();

            return processRunner;
        }

        private ProcessRunner(string exePath, string arguments)
        {
            processStartInfo = new ProcessStartInfo(exePath)
            {
                Arguments = arguments,
                LoadUserProfile = false,
                UseShellExecute = false
            };
        }

        public void Start()
        {
            process = Process.Start(processStartInfo);
        }

        public void Dispose()
        {
            if (process == null) return;

            process.Kill();
        }
    }
}
