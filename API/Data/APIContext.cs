using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API;

namespace API.Data
{
    public class APIContext : DbContext
    {
        public APIContext (DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<API.Cidade> Cidade { get; set; } = default!;

        public DbSet<API.Endereco>? Endereco { get; set; }

        public DbSet<API.Cliente>? Cliente { get; set; }

        public DbSet<API.Hotel>? Hotel { get; set; }

        public DbSet<API.Passagem>? Passagem { get; set; }

        public DbSet<API.Pacote>? Pacote { get; set; }
    }
}
