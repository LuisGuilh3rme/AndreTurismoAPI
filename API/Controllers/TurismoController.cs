using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TurismoController : ControllerBase
    {
        [HttpPost(Name = "Insert")]
        public void Insert(Pacote pacote)
        {
            new TurismoService().Inserir(pacote);
        }

        [HttpGet(Name = "Get")]
        public List<Pacote> FindAll()
        {
            return new TurismoService().FindAll();
        }
    }
}
