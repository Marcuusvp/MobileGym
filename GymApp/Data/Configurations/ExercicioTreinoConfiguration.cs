using GymApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymApp.Data.Configurations;

public class ExercicioTreinoConfiguration : IEntityTypeConfiguration<ExercicioTreino>
{
    public void Configure(EntityTypeBuilder<ExercicioTreino> builder)
    {
        builder.ToTable("EXERCICIO_TREINO");
        builder.HasKey(et => et.Id);

        builder.HasOne(et => et.Treino)
            .WithMany(t => t.Exercicios)
            .HasForeignKey(et => et.TreinoId);

        builder.HasOne(et => et.Exercicio)
            .WithMany()
            .HasForeignKey(et => et.ExercicioId);

        builder.Property(et => et.Carga).IsRequired();
        builder.Property(et => et.Series).IsRequired();
        builder.Property(et => et.Repeticoes).IsRequired();
    }
}
