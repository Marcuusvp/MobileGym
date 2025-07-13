using GymApp.Data;
using GymApp.Dto;
using GymApp.Models;
using GymApp.Servico.Provedores;

namespace GymApp.Servico;

public class CreateExercicioHandler
{
    private readonly GymAppDbContext _db;
    private readonly ILogger<CreateExercicioHandler> _logger;
    private readonly IImageStorageService _imageStorageService;

    public CreateExercicioHandler(IImageStorageService imageStorageService, ILogger<CreateExercicioHandler> logger, GymAppDbContext db)
    {
        _imageStorageService = imageStorageService;
        _logger = logger;
        _db = db;
    }

    public async Task<Exercicio> CriarAsync(CreateExercicioRequest request)
    {
        var imageParams = new NewImageDto();

        if (request.Imagem != null && request.Imagem.Length > 0)
            imageParams = await _imageStorageService.UploadImageAsync(request.Imagem);


        var exercicio = Exercicio.Criar(
            nome: request.Nome,
            series: request.Series,
            repeticoes: request.Repeticoes,
            carga: request.Carga,
            foto: imageParams.ImageUrl,
            video: request.Video,
            publicId: imageParams.PublicId
        );

        _db.Exercicios.Add(exercicio);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Exercício criado com ID {Id}", exercicio.Id);

        return exercicio;
    }
}
