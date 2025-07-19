namespace GymApp.Models;

public class Treino
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public ICollection<ExercicioTreino> Exercicios { get; private set; }
    public string Usuario { get; private set; } = string.Empty;

    private Treino() { }

    public static Treino Criar(string nome, List<ExercicioTreino> exercicios, string usuario)
    {
        return new Treino
        {
            Id = new Guid(),
            Nome = nome,
            Exercicios = exercicios,
            Usuario = usuario
        };
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
        {
            throw new ArgumentException("O nome do treino não pode ser vazio ou nulo.");
        }
        Nome = novoNome;
    }
}
