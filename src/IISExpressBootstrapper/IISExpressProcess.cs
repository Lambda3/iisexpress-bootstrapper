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
                RedirectStandardOutput = output != null
            };

            if (configuration.EnvironmentVariables != null)
                foreach (var environmentVariable in configuration.EnvironmentVariables)
                    processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
        }

        private static string GetDefaultIISExpressPath(bool preferX64)
        {
            var iisExpressPath = $@"{Environment.GetEnvironmentVariable("ProgramFiles")}\IIS Express\IISExpress.exe";
            string X64Path() => File.Exists(iisExpressPath) ? iisExpressPath : throw new IISExpressNotFoundException(iisExpressPath);
            if (preferX64)
                return X64Path();
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
            if (processStartInfo.RedirectStandardOutput)
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
