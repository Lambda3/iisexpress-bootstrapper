using System;
using System.Globalization;
using System.IO;

namespace IISExpressBootstrapper
{
    internal class IISExpressProcess : IDisposable
    {
        private readonly ProcessRunner process;

        public IISExpressProcess(WebApplication webApplication)
        {
            const string iisExpressX86Path = @"c:\program files (x86)\IIS Express\IISExpress.exe";
            const string iisExpressX64Path = @"c:\program files\IIS Express\IISExpress.exe";
            var processFileName = File.Exists(iisExpressX86Path) ? iisExpressX86Path : iisExpressX64Path;

            var arguments = string.Format(@"/systray:false /trace:true /path:{0} /port:{1}", webApplication.FullPath, webApplication.PortNumber.ToString(CultureInfo.InvariantCulture));

            process = ProcessRunner.Run(processFileName, arguments);
        }

        public void Dispose()
        {
            process.Dispose();
        }
    }
}
