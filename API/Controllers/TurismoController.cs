using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;

namespace Turismo.Controllers
{
    public class TurismoController
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
