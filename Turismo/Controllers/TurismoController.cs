using Turismo.Models;
using Turismo.Services;

namespace Turismo.Controllers
{
    internal class TurismoController
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
