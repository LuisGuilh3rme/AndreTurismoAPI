using API.Models;
using API.Repositories;

namespace API.Services
{
    public class TurismoService
    {

        public void AtualizarCampo(int id, string tabela, string campo, string atualizarString)
        {
            new TurismoRepository().AtualizarCampo(id, tabela, campo, atualizarString);
        }

        public void RemoverPacote(int id)
        {
            new TurismoRepository().RemoverPacote(id);
        }

        public bool Inserir(Pacote pacote)
        {
            return new TurismoRepository().Inserir(pacote);
        }

        public List<Pacote> FindAll()
        {
            return new TurismoRepository().FindAll();
        }
    }
}
