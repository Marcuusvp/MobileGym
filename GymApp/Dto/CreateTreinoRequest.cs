namespace GymApp.Dto;

public class CreateTreinoRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public List<ExercicioDoTreinoDto> Exercicios { get; set; } = new();
}

public class ExercicioDoTreinoDto
{
    public Guid ExercicioId { get; set; }
    public int Series { get; set; }
    public int Repeticoes { get; set; }
    public decimal Carga { get; set; }
}
