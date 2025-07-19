namespace GymApp.Dto;

public class UpdateTreinoRequest
{
    public string? Nome { get; set; }
    public List<ExercicioTreinoUpdateDto>? Exercicios { get; set; }
}

public class ExercicioTreinoUpdateDto
{
    public Guid ExercicioId { get; set; }
    public int Series { get; set; }
    public int Repeticoes { get; set; }
    public decimal Carga { get; set; }
}
