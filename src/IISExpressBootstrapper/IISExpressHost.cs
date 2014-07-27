using System;

namespace IISExpressBootstrapper
{
    public class IISExpressHost : IDisposable
    {
        private readonly IISExpressProcess process;

        public IISExpressHost(string webApplicationName, int portNumber)
        {
            process = new IISExpressProcess(new WebApplication(webApplicationName, portNumber));
        }

        public void Dispose()
        {
            process.Dispose();
        }
    }
}
