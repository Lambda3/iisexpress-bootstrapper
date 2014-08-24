using System.Globalization;
using System.Text;

namespace IISExpressBootstrapper
{
    /// <summary>
    /// Trace level for IIS Express
    /// </summary>
    public enum TraceLevel
    {
        Info,
        Warning,
        Error
    }

    /// <summary>
    /// Parameters base class.
    /// </summary>
    public abstract class Parameters
    {
        /// <summary>
        /// Enables or disables the system tray application. The default value is false.
        /// </summary>
        public bool? Systray { get; set; }

        /// <summary>
        /// Specify the debug trace level.
        /// </summary>
        public TraceLevel? TraceLevel { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (TraceLevel.HasValue)
                sb.Append(" /trace:" + TraceLevel.ToString().ToLower());

            if (Systray.HasValue)
                sb.Append(" /systray:" + Systray.Value.ToString().ToLower());

            return sb.ToString();
        }
    }

    /// <summary>
    /// Specify the parameters for run your site from a configuration file.
    /// </summary>
    public class ConfigFileParameters : Parameters
    {
        /// <summary>
        /// The full path to the applicationhost.config file.
        /// </summary>
        public string ConfigFile { get; set; }

        /// <summary>
        /// The name of the site to launch, as described in the applicationhost.config file.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// The ID of the site to launch, as described in the applicationhost.config file.
        /// </summary>
        public string SiteId { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(ConfigFile))
                sb.Append(string.Format(@" /config:""{0}""", ConfigFile));

            if (!string.IsNullOrEmpty(SiteName))
                sb.Append(string.Format(@" /site:""{0}""", SiteName));

            if (!string.IsNullOrEmpty(SiteId))
                sb.Append(string.Format(@" /siteid:""{0}""", SiteId));

            sb.Append(base.ToString());

            return sb.ToString();
        }
    }

    /// <summary>
    /// Specify the parameters for run your site from the application folder.
    /// </summary>
    public class PathParameters : Parameters
    {
        /// <summary>
        /// The full physical path of the application to run.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The port to which the application will bind. The default value is 8080. You must also specify the Path option.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The .NET Framework version (e.g. v4.0) to use to run the application.
        /// </summary>
        public string ClrVersion { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Port != 0)
                sb.Append(" /port:" + Port.ToString(CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(Path))
                sb.Append(string.Format(@" /path:""{0}""", Path));

            if (!string.IsNullOrEmpty(ClrVersion))
                sb.Append(string.Format(@" /clr:""{0}""", ClrVersion));

            sb.Append(base.ToString());

            return sb.ToString();
        }
    }
}
