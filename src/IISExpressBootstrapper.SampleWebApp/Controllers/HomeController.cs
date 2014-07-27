using System.Web.Mvc;

namespace IISExpressBootstrapper.SampleWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("It work's!");
        }
    }
}
