using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Models
{
    internal class Cidade
    {
        public readonly static string INSERT = "INSERT INTO Cidade (Nome) VALUES (@Nome)";
        public readonly static string GETALL = "SELECT * FROM Cidade";
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
