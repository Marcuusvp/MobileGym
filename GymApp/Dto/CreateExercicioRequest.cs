namespace GymApp.Dto;

public class CreateExercicioRequest
{
    public string Nome { get; set; } = string.Empty;
    public IFormFile? Imagem { get; set; }
    public string Video { get; set; } = string.Empty;    
}
