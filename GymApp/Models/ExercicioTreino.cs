using System.Text.Json.Serialization;

namespace GymApp.Models;

public class ExercicioTreino
{
    public Guid Id { get; private set; }
    public Guid TreinoId { get; private set; }
    [JsonIgnore]
    public Treino Treino { get; private set; } = null!;

    public Guid ExercicioId { get; private set; }
    [JsonIgnore]
    public Exercicio Exercicio { get; private set; } = null!;

    public int Series { get; private set; }
    public int Repeticoes { get; private set; }
    public decimal Carga { get; private set; }

    private ExercicioTreino() { }

    public static ExercicioTreino Criar(Guid exercicioId, int series, int repeticoes, decimal carga)
    {
        return new ExercicioTreino
        {
            Id = Guid.NewGuid(),
            ExercicioId = exercicioId,
            Series = series,
            Repeticoes = repeticoes,
            Carga = carga
        };
    }
    public void Atualizar(int series, int repeticoes, decimal carga)
    {
        Series = series;
        Repeticoes = repeticoes;
        Carga = carga;
    }
}
