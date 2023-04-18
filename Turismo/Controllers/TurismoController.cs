using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            // return new TurismoService().ListarPacotes();
            return null;
        }
    }
}
