using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TurismoController : ControllerBase
    {
        [HttpPost(Name = "Insert")]
        public ActionResult Insert(Pacote pacote)
        {
            if (new TurismoService().Inserir(pacote)) return StatusCode(200);
            return BadRequest();
        }

        [HttpGet(Name = "Get")]
        public List<Pacote> FindAll()
        {
            return new TurismoService().FindAll();
        }
    }
}
