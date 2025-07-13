namespace GymApp.Dto;

public class CreateTreinoRequest
{
    public string Nome { get; set; } = string.Empty;
    public List<Guid> ExerciciosIds { get; set; } = new();
}
