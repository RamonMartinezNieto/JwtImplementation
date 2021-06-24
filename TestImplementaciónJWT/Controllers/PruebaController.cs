using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestImplementaciónJWT.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PruebaController : ControllerBase
    {

        [HttpGet("ping")]
        public IActionResult ping() 
        {
            return Ok(true);
        }


        [HttpGet("datos")]
        public IActionResult GetData() 
        {
            Dictionary<string, string> algo = new Dictionary<string,string>();
            algo.Add("ramon", "martinez");
            algo.Add("marina", "amer");

            return Ok(algo);
        }

    }
}
