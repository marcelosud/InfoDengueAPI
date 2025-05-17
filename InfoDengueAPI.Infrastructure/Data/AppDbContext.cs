using Microsoft.EntityFrameworkCore;
using InfoDengueAPI.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InfoDengueAPI.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }

        public DbSet<DadoEpidemiologico> DadosEpidemiologicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DadoEpidemiologico>()
                .HasOne(d => d.Relatorio)
                .WithMany(r => r.DadosEpidemiologicos)
                .HasForeignKey(d => d.RelatorioId);
        }

    }
}
