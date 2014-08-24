using System;
using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public class IISExpressHost : IDisposable
    {
        public static IISExpressHost Start(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null)
        {
            return new IISExpressHost(webApplicationName, portNumber, environmentVariables, iisExpressPath);
        }

        public static IISExpressHost Start(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null)
        {
            return new IISExpressHost(parameters, environmentVariables, iisExpressPath);
        }

        private IISExpressProcess process;

        public IISExpressHost(string webApplicationName, int portNumber,
            IDictionary<string, string> environmentVariables = null, string iisExpressPath = null)
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
                }
            };

            process = new IISExpressProcess(configuration);
        }

        private IISExpressHost(Parameters parameters, IDictionary<string, string> environmentVariables = null,
            string iisExpressPath = null)
        {
            var configuration = new Configuration
            {
                EnvironmentVariables = environmentVariables,
                IISExpressPath = iisExpressPath,
                ProcessParameters = parameters
            };
            process = new IISExpressProcess(configuration);
        }

        public void Dispose()
        {
            if (process == null) return;

            var toDispose = process;
            process = null;

            toDispose.Dispose();
        }

        ~IISExpressHost()
        {
            Dispose();
        }
    }
}
