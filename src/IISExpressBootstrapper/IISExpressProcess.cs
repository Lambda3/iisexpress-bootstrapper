using System;
using System.Diagnostics;
using System.IO;

namespace IISExpressBootstrapper
{
    internal sealed class IISExpressProcess : IDisposable
    {
        private readonly ProcessStartInfo processStartInfo;
        private Process process;
        private readonly Action<string> output;
        public bool IsRunning => process != null && !process.HasExited;
        public int ProcessId => process == null ? -1 : process.Id;
        public string IISExpressPath { get; private set; }
        public IISExpressProcess(Configuration configuration)
        {
            IISExpressPath = string.IsNullOrWhiteSpace(configuration.IISExpressPath)
            ? GetDefaultIISExpressPath(configuration.PreferX64)
            : configuration.IISExpressPath;
            if (!File.Exists(IISExpressPath))
                throw new IISExpressNotFoundException(IISExpressPath);

            output = configuration.Output;

            processStartInfo = new ProcessStartInfo(IISExpressPath)
            {
                Arguments = configuration.ProcessParameters.ToString(),
                LoadUserProfile = false,
                UseShellExecute = false,
                RedirectStandardOutput = output != null,
                RedirectStandardError = output != null
            };
            if (configuration.EnvironmentVariables != null)
                foreach (var environmentVariable in configuration.EnvironmentVariables)
                    processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
        }

        public event EventHandler Exited;

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) => output(e.Data);

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) => output(e.Data);

        private static string GetDefaultIISExpressPath(bool preferX64)
        {
            string X64Path()
            {
                var iisExpressPath = $@"{Environment.GetEnvironmentVariable("ProgramW6432")}\IIS Express\IISExpress.exe";
                return File.Exists(iisExpressPath) ? iisExpressPath : throw new IISExpressNotFoundException(iisExpressPath);
            }
            if (preferX64)
            {
                if (!Environment.Is64BitOperatingSystem)
                    throw new Exception("Can't prefer x64 IIS in 32 bits system.");
                return X64Path();
            }
            var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (programFilesX86 == null)
                return X64Path();
            var iisExpressX86Path = $@"{programFilesX86}\IIS Express\IISExpress.exe";
            return File.Exists(iisExpressX86Path)
                ? iisExpressX86Path
                : X64Path();
        }

        public void Start()
        {
            process = Process.Start(processStartInfo);
            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;
            if (processStartInfo.RedirectStandardOutput)
            {
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
            }
        }

        private void Process_Exited(object sender, EventArgs e) => Exited?.Invoke(this, e);

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
