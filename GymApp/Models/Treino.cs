namespace GymApp.Models;

public class Treino
{
    public Guid Id { get; }
    public string Nome { get; } = string.Empty;
    public List<Exercicio> Exercicios { get; } = new List<Exercicio>();
}
