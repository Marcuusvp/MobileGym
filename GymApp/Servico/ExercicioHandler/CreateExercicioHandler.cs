using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;
using GymApp.Servico.Provedores;

namespace GymApp.Servico.ExercicioHAndler;

public class CreateExercicioHandler
{
    private readonly IExercicioRepository _repo;
    private readonly ILogger<CreateExercicioHandler> _logger;
    private readonly IImageStorageService _imageStorageService;

    public CreateExercicioHandler(IImageStorageService imageStorageService, ILogger<CreateExercicioHandler> logger, IExercicioRepository repo)
    {
        _imageStorageService = imageStorageService;
        _logger = logger;
        _repo = repo;
    }

    public async Task<Exercicio> CriarAsync(CreateExercicioRequest request)
    {
        var imageParams = new NewImageDto();

        if (request.Imagem != null && request.Imagem.Length > 0)
            imageParams = await _imageStorageService.UploadImageAsync(request.Imagem);


        var exercicio = Exercicio.Criar(
            nome: request.Nome,
            foto: imageParams.ImageUrl,
            video: request.Video,
            publicId: imageParams.PublicId
        );

        await _repo.AddAsync(exercicio);

        await _repo.SaveChangesAsync();

        _logger.LogInformation("Exercício criado com ID {Id}", exercicio.Id);

        return exercicio;
    }
}
