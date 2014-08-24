using System;

namespace IISExpressBootstrapper
{
    public class IISExpressNotFoundException : Exception
    {
        public IISExpressNotFoundException()
            : base("Could not find iisexpress.exe path.")
        {
        }
    }
}
