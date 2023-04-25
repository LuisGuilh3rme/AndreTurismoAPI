using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Endereco
    {
        public readonly static string INSERT = $"INSERT INTO Endereco (Logradouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro) VALUES (@Logradouro, @Numero, @Bairro, @CEP, @Complemento, @Cidade, @DataCadastro);";
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        [ForeignKey("Fk_Endereco_Cidade")]
        public Cidade Cidade { get; set; }
        public DateTime DataCadastro { get; set; }

        public override string ToString()
        {
            return $"\n Rua: {Logradouro} \n Numero: {Numero} \n Bairro: {Bairro} \n CEP: {CEP} \n Complemento: {Complemento} \n Cidade: {Cidade.Nome} \n Data de cadastro: {DataCadastro}";
        }
    }
}
