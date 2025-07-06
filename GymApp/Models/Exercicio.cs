namespace GymApp.Models;

public class Exercicio
{
    public Guid Id { get; }
    public string Nome { get; } = string.Empty;
    public int Series { get; }
    public int Repeticoes { get; }
    public decimal Carga { get; }
    public string Foto { get; } = string.Empty;
    public string Video { get; } = string.Empty;
}
