namespace Turismo.Models
{
    internal class Cidade
    {
        public static readonly string INSERT = "INSERT INTO Cidade (Nome) VALUES (@Nome);";
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
