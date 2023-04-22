using System.Reflection.Metadata.Ecma335;

namespace API
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"\n Nome do hotel: {Nome} \n Endereço Hotel: {Endereco} \n Valor: {Valor}";
        }
    }
}
