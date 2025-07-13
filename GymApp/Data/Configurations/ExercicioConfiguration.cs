using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Data.Configurations;

public class ExercicioConfiguration : IEntityTypeConfiguration<Exercicio>
{
    public void Configure(EntityTypeBuilder<Exercicio> builder)
    {
        builder.ToTable("EXERCICIOS");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(e => e.Foto)
            .HasColumnType("varchar(255)");

        builder.Property(e => e.Video)
            .HasColumnType("varchar(255)");

        builder.Property(e => e.ImagePublicId)
            .HasColumnType("varchar(255)");
    }
}
