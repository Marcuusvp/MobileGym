using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;
using GymApp.Servico.Provedores;

namespace GymApp.Servico.ExercicioHandler;

public class UpdateExercicioHandler
{
    private readonly IExercicioRepository _repo;


    private readonly ILogger<UpdateExercicioHandler> _logger;
    private readonly IImageStorageService _imageStorageService;

    public UpdateExercicioHandler(IExercicioRepository repo, ILogger<UpdateExercicioHandler> logger, IImageStorageService imageStorageService)
    {
        _repo = repo;
        _logger = logger;
        _imageStorageService = imageStorageService;
    }

    public async Task<Exercicio?> AtualizarAsync(Guid id, UpdateExercicioRequest request)
    {
        var exercicio = await _repo.GetByIdAsync(id);
        if (exercicio == null)
        {
            _logger.LogWarning("Exercício com ID {Id} não encontrado para atualização.", id);
            return null;
        }
        if (!string.IsNullOrEmpty(request.Nome))
        {
            exercicio.AtualizarNome(request.Nome);
        }

        if (!string.IsNullOrEmpty(request.Video))
        {
            exercicio.AtualizarVideo(request.Video);
        }

        if (request.Imagem != null && request.Imagem.Length > 0)
        {
            if (!string.IsNullOrEmpty(exercicio.ImagePublicId))
            {
                var deleted = await _imageStorageService.DeleteImageAsync(exercicio.ImagePublicId);
                if (!deleted)
                {
                    _logger.LogWarning("Falha ao deletar imagem anterior do Cloudinary para o exercício {Id} (PublicId: {PublicId}).", exercicio.Id, exercicio.ImagePublicId);
                    // TODO: O QUE FAZER SE DER ERRO
                }
            }

            var newImageParams = await _imageStorageService.UploadImageAsync(request.Imagem);
            exercicio.AtualizarFoto(newImageParams.ImageUrl, newImageParams.PublicId);
        }

        await _repo.UpdateAsync(exercicio);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Exercício com ID {Id} atualizado com sucesso.", exercicio.Id);

        return exercicio;
    }

}
