using API.Models;

namespace Turismo.Services
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

        public void Inserir(Pacote pacote)
        {
            new TurismoRepository().Inserir(pacote);
        }

        public List<Pacote> FindAll()
        {
            return new TurismoRepository().FindAll();
        }
    }
}
