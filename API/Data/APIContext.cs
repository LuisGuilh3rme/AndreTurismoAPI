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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Endereco>().HasOne(e => e.Cidade).WithOne().HasForeignKey<Endereco>("CidadeId").OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Cliente>().HasOne(c => c.Endereco).WithOne().HasForeignKey<Cliente>("EnderecoId").OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Hotel>().HasOne(h => h.Endereco).WithOne().HasForeignKey<Hotel>("EnderecoId").OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Passagem>().HasOne(p => p.Origem).WithOne().HasForeignKey<Passagem>("OrigemId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Passagem>().HasOne(p => p.Destino).WithOne().HasForeignKey<Passagem>("DestinoId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Passagem>().HasOne(p => p.Cliente).WithOne().HasForeignKey<Passagem>("ClienteId").OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Pacote>().HasOne(p => p.Hotel).WithOne().HasForeignKey<Pacote>("HotelId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pacote>().HasOne(p => p.Passagem).WithOne().HasForeignKey<Pacote>("PassagemId").OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Pacote>().HasOne(p => p.Cliente).WithOne().HasForeignKey<Pacote>("ClienteId").OnDelete(DeleteBehavior.NoAction);
        }
    }
}
