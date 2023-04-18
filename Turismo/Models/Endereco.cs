using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Models
{
    internal class Endereco
    {
        public readonly static string INSERT = "INSERT INTO Endereco (Logradouro, Numero, Bairro, CEP, Complemento, Id_Cidade, Data_Cadastro) VALUES (@Logradouro, @Numero, @Bairro, @CEP, @Complemento, @Id_Cidade, @Data_Cadastro)";
        public readonly static string GETALL = "SELECT * FROM Endereco";
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public int Numero { get; set; }
        public string Bairro { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public Cidade Cidade { get; set; }
        public DateTime DataCadastro { get; set; }

        public override string ToString()
        {
            return $"\n Rua: {Logradouro} \n Numero: {Numero} \n Bairro: {Bairro} \n CEP: {CEP} \n Complemento: {Complemento} \n Cidade: {Cidade.Nome} \n Data de cadastro: {DataCadastro}";
        }
    }
}
