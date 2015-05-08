using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConsoleHost.Service.Controllers
{
    public class TestController : ApiController
    {
       [HttpGet]
       public List<string> Get()
       {
           return new List<string>() { "Test", "Test2" };
       }
    }
}
