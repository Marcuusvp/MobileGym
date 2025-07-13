namespace GymApp.Dto;

public class CreateExercicioRequest
{
    public string Nome { get; set; } = string.Empty;
    public int Series { get; set; }
    public int Repeticoes { get; set; }
    public decimal Carga { get; set; }
    public IFormFile? Imagem { get; set; }
    public string Video { get; set; } = string.Empty;    
}
