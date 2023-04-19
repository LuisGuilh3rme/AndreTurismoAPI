using Turismo.Models;

namespace Turismo.Repositories
{
    internal interface ITurismoRepository
    {
        int InserirCidade(Cidade cidade);
        int InserirEndereco(Endereco endereco);
        int InserirCliente(Cliente cliente);
        int InserirPassagem(Passagem passagem);
        int InserirHotel(Hotel hotel);
        void Inserir(Pacote pacote);
        void AtualizarCampo(int id, string tabela, string campo, string atualizarString);
        void RemoverPacote(int id);
        List<Pacote> FindAll();
        Cidade RetornarCidade(int id);
        Endereco RetornarEndereco(int id);
        Cliente RetornarCliente(int id);
        Passagem RetornarPassagem(int id);
        Hotel RetornarHotel(int id);

    }
}
