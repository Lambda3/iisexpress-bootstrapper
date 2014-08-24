using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IISExpressBootstrapper
{
    internal class ProcessRunner : IDisposable
    {
        private readonly ProcessStartInfo processStartInfo;
        private Process process;

        public static ProcessRunner Run(string exePath, string arguments = "", IDictionary<string, string> environmentVariables = null)
        {
            var processRunner = new ProcessRunner(exePath, arguments, environmentVariables);
            processRunner.Start();

            return processRunner;
        }

        private ProcessRunner(string exePath, string arguments, IEnumerable<KeyValuePair<string, string>> environmentVariables)
        {
            processStartInfo = new ProcessStartInfo(exePath)
            {
                Arguments = arguments,
                LoadUserProfile = false,
                UseShellExecute = false
            };
            
            if (environmentVariables == null) return;
            
            foreach (var environmentVariable in environmentVariables)
            {
                processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
            }
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
