using System.Collections.Generic;

namespace IISExpressBootstrapper
{
    public class Configuration
    {
        public string IISExpressPath { get; set; }

        public Parameters ProcessParameters { get; set; }

        public IDictionary<string, string> EnvironmentVariables { get; set; }
    }
}
