using Microsoft.EntityFrameworkCore;
using Loteria.Core.Entities;

namespace LoteriaAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<TipoLoteria> Loterias { get; set; }
    public DbSet<Concurso> Concursos { get; set; }
    public DbSet<NumerosGerados> NumerosGerados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relacionamentos
        modelBuilder.Entity<Concurso>()
            .HasOne(c => c.TipoLoteria)
            .WithMany(l => l.Concursos)
            .HasForeignKey(c => c.TipoLoteriaId);

        modelBuilder.Entity<NumerosGerados>()
            .HasOne(n => n.Usuario)
            .WithMany(u => u.NumerosGerados)
            .HasForeignKey(n => n.UsuarioId);

        modelBuilder.Entity<NumerosGerados>()
            .HasOne(n => n.TipoLoteria)
            .WithMany(l => l.NumerosGerados)
            .HasForeignKey(n => n.TipoLoteriaId);

        // Índices
        modelBuilder.Entity<Concurso>()
            .HasIndex(c => new { c.TipoLoteriaId, c.NumeroConcurso })
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Seed inicial - Todas as Loterias
        modelBuilder.Entity<TipoLoteria>().HasData(
            new TipoLoteria { Id = 1, Nome = "MEGA_SENA", QuantidadeNumeros = 6, MinNumero = 1, MaxNumero = 60 },
            new TipoLoteria { Id = 2, Nome = "LOTOFACIL", QuantidadeNumeros = 15, MinNumero = 1, MaxNumero = 25 },
            new TipoLoteria { Id = 3, Nome = "QUINA", QuantidadeNumeros = 5, MinNumero = 1, MaxNumero = 80 },
            new TipoLoteria { Id = 4, Nome = "LOTOMANIA", QuantidadeNumeros = 50, MinNumero = 1, MaxNumero = 100 },
            new TipoLoteria { Id = 5, Nome = "DUPLA_SENA", QuantidadeNumeros = 6, MinNumero = 1, MaxNumero = 50 },
            new TipoLoteria { Id = 6, Nome = "TIMEMANIA", QuantidadeNumeros = 10, MinNumero = 1, MaxNumero = 80 },
            new TipoLoteria { Id = 7, Nome = "DIADESORTE", QuantidadeNumeros = 7, MinNumero = 1, MaxNumero = 31 }
        );
    }
}
