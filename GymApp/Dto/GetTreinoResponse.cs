
namespace GymApp.Dto;

public class GetTreinoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<ExercicioDoTreinoDto> Exercicios { get; set; } = new();
}
