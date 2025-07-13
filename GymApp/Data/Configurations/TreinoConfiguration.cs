
using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Data.Configurations;

public class TreinoConfiguration : IEntityTypeConfiguration<Treino>
{
    public void Configure(EntityTypeBuilder<Treino> builder)
    {
        builder.ToTable("TREINOS");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(t => t.Usuario)
            .IsRequired()
            .HasColumnType("varchar(50)");
    }
}
