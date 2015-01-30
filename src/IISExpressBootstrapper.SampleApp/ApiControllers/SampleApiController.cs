using System.Web.Http;

namespace IISExpressBootstrapper.SampleApp.ApiControllers
{
    public class SampleApiController : ApiController
    {
        // GET api/<controller>/5
        public string Get(int id)
        {
            return string.Format("You send me {0}", id);
        }
    }
}
