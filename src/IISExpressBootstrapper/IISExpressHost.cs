using System;
using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public class IISExpressHost : IDisposable
    {
        private readonly IISExpressProcess process;

        public IISExpressHost(string webApplicationName, int portNumber, IDictionary<string, string> environmentVariables = null, string iisExpressPath = null)
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

        public IISExpressHost(Configuration configuration)
        {
            process = new IISExpressProcess(configuration);
        }

        public void Dispose()
        {
            process.Dispose();
        }
    }
}
