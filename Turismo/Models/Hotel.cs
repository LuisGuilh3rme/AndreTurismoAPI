using System.Reflection.Metadata.Ecma335;

namespace Turismo.Models
{
    internal class Hotel
    {
        public readonly static string INSERT = "INSERT INTO Hotel (Nome, Id_Endereco, Data_Cadastro, Valor) VALUES (@Nome, @Id_Endereco, @Data_Cadastro, @Valor)";
        public readonly static string GETALL = "SELECT * FROM Hotel";
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
