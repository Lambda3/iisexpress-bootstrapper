using System;
using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public sealed class IISExpressHost : IDisposable
    {
        public static IISExpressHost Start(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null, Action<string> output = null) =>
            new IISExpressHost(webApplicationName, portNumber, environmentVariables, iisExpressPath, output);

        public static IISExpressHost Start(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null, Action<string> output = null) =>
            new IISExpressHost(parameters, environmentVariables, iisExpressPath, output);

        private IISExpressProcess process;

        public bool IsRunning => process.IsRunning;

        public int ProcessId => process.ProcessId;

        public IISExpressHost(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null, Action<string> output = null)
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
                Output = output
            };

            process = new IISExpressProcess(configuration);
        }

        private IISExpressHost(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null, Action<string> output = null)
        {
            var configuration = new Configuration
            {
                EnvironmentVariables = environmentVariables,
                IISExpressPath = iisExpressPath,
                ProcessParameters = parameters,
                Output = output
            };
            process = new IISExpressProcess(configuration);
        }

        public void Dispose()
        {
            if (process == null)
                return;

            var toDispose = process;
            process = null;

            toDispose.Dispose();
        }
    }
}
