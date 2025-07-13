namespace GymApp.Models;

public class Exercicio
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Foto { get; private set; } = string.Empty;
    public string Video { get; private set; } = string.Empty;
    public string ImagePublicId { get; private set; } = string.Empty;

    private Exercicio() { }
    public static Exercicio Criar(string nome, string foto, string video, string publicId)
    {
        return new Exercicio
        {
            Id = new Guid(),
            Nome = nome,
            Foto = foto,
            Video = video,
            ImagePublicId = publicId
        };
    }
}
