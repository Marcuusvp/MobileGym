

using GymApp.Repositorio.Interface;
using GymApp.Servico.Provedores;

namespace GymApp.Servico.ExercicioHandler;

public class DeleteExercicioHandler
{
    private readonly IExercicioRepository _repo;
    private readonly ILogger<DeleteExercicioHandler> _logger;
    private readonly IImageStorageService _imageStorageService;

    public DeleteExercicioHandler(IExercicioRepository repo, ILogger<DeleteExercicioHandler> logger, IImageStorageService imageStorageService)
    {
        _repo = repo;
        _logger = logger;
        _imageStorageService = imageStorageService;
    }

    public async Task<bool> DeletarAsync(Guid id)
    {
        var exercicio = await _repo.GetByIdAsync(id);

        if (exercicio == null)
        {
            _logger.LogWarning("Exercício com ID {Id} não encontrado para deleção.", id);
            return false;
        }

        if (!string.IsNullOrEmpty(exercicio.ImagePublicId))
        {
            var deleted = await _imageStorageService.DeleteImageAsync(exercicio.ImagePublicId);
            if (!deleted)
            {
                _logger.LogWarning("Falha ao deletar imagem do Cloudinary para o exercício {Id} (PublicId: {PublicId}).", exercicio.Id, exercicio.ImagePublicId);
                //TODO: EXERCICIO DELETA MESMO COM IMAGEM TENDO PROBLEMA?
            }
        }

        await _repo.DeleteAsync(exercicio);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Exercício com ID {Id} deletado com sucesso.", exercicio.Id);

        return true;
    }

}
