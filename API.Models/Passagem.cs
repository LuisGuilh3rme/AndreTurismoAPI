using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Passagem
    {
        public readonly static string INSERT = "INSERT INTO Passagem (Id_Origem, Id_Destino, Id_Cliente, Data, Valor) VALUES (@Origem, @Destino, @Cliente, @Data, @Valor);";
        public int Id { get; set; }

        [ForeignKey("Fk_Passagem_Origem")]
        public Endereco Origem { get; set; }

        [ForeignKey("Fk_Passagem_Destino")]
        public Endereco Destino { get; set; }

        [ForeignKey("Fk_Passagem_Cliente")]
        public Cliente Cliente { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"Origem: {Origem} \n Destino: {Destino} \n Cliente: {Cliente} \n Data: {Data} \n Valor: {Valor}";
        }
    }
}
