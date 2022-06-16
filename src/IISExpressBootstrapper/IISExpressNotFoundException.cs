using System;

namespace IISExpressBootstrapper
{
    public class IISExpressNotFoundException : Exception
    {
        public IISExpressNotFoundException(string path)
            : base($"Could not find iisexpress.exe path at '{path}'.") { }
    }
}
