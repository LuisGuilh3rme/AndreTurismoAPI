using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TurismoController : ControllerBase
    {
        public void Insert(Pacote pacote)
        {
            new TurismoService().Inserir(pacote);
        }

        public List<Pacote> FindAll()
        {
            return new TurismoService().FindAll();
        }
    }
}
