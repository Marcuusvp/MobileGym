namespace GymApp.Models;

public class Exercicio
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public int Series { get; private set; }
    public int Repeticoes { get; private set; }
    public decimal Carga { get; private set; }
    public string Foto { get; private set; } = string.Empty;
    public string Video { get; private set; } = string.Empty;

    private Exercicio(){}
    public Exercicio Criar(string nome, int series, int repeticoes, decimal carga, string foto, string video)
    {
        return new Exercicio
        {
            Id = new Guid(),
            Nome = nome,
            Series = series,
            Repeticoes = repeticoes,
            Carga = carga,
            Foto = foto,
            Video = video
        };
    }
}
