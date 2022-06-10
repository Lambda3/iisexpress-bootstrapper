using System.Web.Http;

namespace IISExpressBootstrapper.SampleApp
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start() => GlobalConfiguration.Configure(Register);
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
