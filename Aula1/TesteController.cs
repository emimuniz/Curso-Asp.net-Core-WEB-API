using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication16.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : ControllerBase
    {
        [HttpGet] 
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "teste1", "teste2" };
        }

        [HttpGet("pessoas")]
        public ActionResult<IEnumerable<Pessoa>> GetPessoa()
        {
            return new[]
            {
                new Pessoa { Nome = "Aliece"},
                new Pessoa { Nome = "Junior"},
                new Pessoa { Nome = "Ednei"}
            };
        }
    }


    public class Pessoa
    {
        public string Nome { get; set; }
    }
}
