using System.Text.Json.Serialization;

namespace API.Models
{
    public class Pacote
    {
        public readonly static string INSERT = "INSERT INTO Pacote (Id_Hotel, Id_Passagem, Data_Cadastro, Valor, Id_Cliente) VALUES (@Hotel, @Passagem, @DataCadastro, @Valor, @Cliente);";
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        public Passagem Passagem { get; set; }
        public DateTime DataCadastro { get; set; }
        public decimal Valor { get; set; }
        public Cliente Cliente { get; set; }

        public override string ToString()
        {
            return $"\n\n Hotel: {Hotel} \n Passagem: {Passagem} \n Data de cadastro: {DataCadastro} \n Valor: {Valor} \n Cliente: {Cliente}";
        }
    }
}
