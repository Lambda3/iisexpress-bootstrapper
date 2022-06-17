using System;
using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public sealed class IISExpressHost : IDisposable
    {
        [Obsolete("Use new IISExpressHost(..).Start().")]
        public static IISExpressHost Start(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null, Action<string> output = null, bool preferX64 = false) =>
            new IISExpressHost(webApplicationName, portNumber, environmentVariables, iisExpressPath, output, preferX64).Start();

        [Obsolete("Use new IISExpressHost(..).Start().")]
        public static IISExpressHost Start(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null, Action<string> output = null, bool preferX64 = false) =>
            new IISExpressHost(parameters, environmentVariables, iisExpressPath, output, preferX64).Start();

        private readonly IISExpressProcess iisProcess;

        public bool IsRunning => iisProcess.IsRunning;

        public int ProcessId => iisProcess.ProcessId;

        public string IISExpressPath => iisProcess.IISExpressPath;

        public event EventHandler Exited;

        public IISExpressHost(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null, Action<string> output = null, bool preferX64 = false)
        {
            var configuration = new Configuration
            {
                IISExpressPath = iisExpressPath,
                EnvironmentVariables = environmentVariables,
                ProcessParameters = new PathParameters
                {
                    Path = Locator.GetFullPath(webApplicationName),
                    Port = portNumber,
                    Systray = false
                },
                Output = output,
                PreferX64 = preferX64
            };
            iisProcess = new IISExpressProcess(configuration);
        }

        public IISExpressHost(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null, Action<string> output = null, bool preferX64 = false)
        {
            var configuration = new Configuration
            {
                EnvironmentVariables = environmentVariables,
                IISExpressPath = iisExpressPath,
                ProcessParameters = parameters,
                Output = output,
                PreferX64 = preferX64
            };
            iisProcess = new IISExpressProcess(configuration);
            iisProcess.Exited += IISProcess_Exited;
        }

        private void IISProcess_Exited(object sender, EventArgs e) => Exited?.Invoke(this, e);

        public IISExpressHost Start()
        {
            if (!iisProcess.IsRunning)
                iisProcess.Start();
            return this;
        }

        public void Dispose() => iisProcess?.Dispose();
    }
}
