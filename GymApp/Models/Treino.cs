namespace GymApp.Models;

public class Treino
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public List<Exercicio> Exercicios { get; private set; } = new List<Exercicio>();

    private Treino() { }

    public Treino Criar(string nome, List<Exercicio> exercicios)
    {
        return new Treino
        {
            Id = new Guid(),
            Nome = nome,
            Exercicios = exercicios
        };
    }
}
