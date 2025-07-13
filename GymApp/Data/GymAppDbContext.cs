using GymApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Data;

public class GymAppDbContext : DbContext
{
    public GymAppDbContext(DbContextOptions<GymAppDbContext> options) : base(options) { }

    public DbSet<Treino> Treinos => Set<Treino>();
    public DbSet<Exercicio> Exercicios => Set<Exercicio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymAppDbContext).Assembly);
    }
}
