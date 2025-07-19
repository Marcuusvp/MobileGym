

namespace GymApp.Dto;

public class UpdateExercicioRequest
{
    public string? Nome { get; set; }
    public string? Video { get; set; }
    public IFormFile? Imagem { get; set; }
}
