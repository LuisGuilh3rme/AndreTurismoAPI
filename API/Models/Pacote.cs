using System.Text.Json.Serialization;

namespace API
{
    public class Pacote
    {
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
