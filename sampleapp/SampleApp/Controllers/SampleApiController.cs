using System;
using System.Web.Http;

namespace IISExpressBootstrapper.SampleApp.Controllers
{
    public class SampleApiController : ApiController
    {
        public string Get(int id) => $"You sent me {id}";
        public string Post(string id) => Environment.GetEnvironmentVariable(id);
    }
}
