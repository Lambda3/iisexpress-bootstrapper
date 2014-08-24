using System;
using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public class IISExpressHost : IDisposable
    {
        private readonly IISExpressProcess process;

        public IISExpressHost(string webApplicationName, int portNumber, string iisExpressPath = null, IDictionary<string, string> environmentVariables = null)
        {
            var webApplication = new WebApplication(webApplicationName);

            var configuration = new Configuration
            {
                IISExpressPath = iisExpressPath,
                EnvironmentVariables = environmentVariables,
                ProcessParameters = new PathParameters
                {
                    Path = webApplication.FullPath,
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
