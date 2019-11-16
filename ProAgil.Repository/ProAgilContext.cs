using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilContext : IdentityDbContext
    {
        public ProAgilContext(DbContextOptions<ProAgilContext> context) : base(context)
        {
        }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Palestrante> Palestrantes { get; set; }
        public DbSet<PalestranteEvento> PalestrantesEventos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<RedeSocial> RedesSocials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Para especificar a relação de N para N entre as tabelas
            modelBuilder.Entity<PalestranteEvento>()
                .HasKey(pe => new {pe.EventoId, pe.PalestranteId});
        }
    }
}