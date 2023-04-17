using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Models
{
    internal class Passagem
    {
        public Endereco Origem { get; set; }
        public Endereco Destino { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
    }
}
