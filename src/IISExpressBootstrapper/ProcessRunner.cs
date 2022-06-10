using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IISExpressBootstrapper
{
    internal sealed class ProcessRunner : IDisposable
    {
        private readonly ProcessStartInfo processStartInfo;
        private Process process;
        private readonly Action<string> output;

        public bool IsRunning => process != null && !process.HasExited;

        public int ProcessId => process == null ? -1 : process.Id;

        public static ProcessRunner Run(string exePath, string arguments = "", IDictionary<string, string> environmentVariables = null, Action<string> output = null)
        {
            var processRunner = new ProcessRunner(exePath, arguments, environmentVariables, output);
            processRunner.Start();

            return processRunner;
        }

        private ProcessRunner(string exePath, string arguments, IEnumerable<KeyValuePair<string, string>> environmentVariables, Action<string> output)
        {
            processStartInfo = new ProcessStartInfo(exePath)
            {
                Arguments = arguments,
                LoadUserProfile = false,
                UseShellExecute = false,
                RedirectStandardOutput = output != null
            };

            this.output = output ?? (_ => { });

            if (environmentVariables == null)
                return;

            foreach (var environmentVariable in environmentVariables)
            {
                processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
            }
        }

        public void Start()
        {
            process = Process.Start(processStartInfo);
            if (process != null && processStartInfo.RedirectStandardOutput)
            {
                var reader = process.StandardOutput;
                while (reader.Peek() > -1)
                {
                    var message = reader.ReadToEnd();
                    output(message);
                }
            }
        }

        public void Dispose()
        {
            if (process == null)
                return;

            try
            {
                if (!process.HasExited)
                    process.Kill();
            }
            catch (InvalidOperationException)
            {
                // Will throw InvalidOperationException if process has already exited.
            }
            process.Dispose();
        }
    }
}
