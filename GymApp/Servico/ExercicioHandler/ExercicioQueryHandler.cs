
using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;

namespace GymApp.Servico.ExercicioHandler;

public class ExercicioQueryHandler
{
    private readonly IExercicioRepository _repo;
    private readonly ILogger<ExercicioQueryHandler> _logger;

    public ExercicioQueryHandler(IExercicioRepository repo, ILogger<ExercicioQueryHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Exercicio?> GetByIdAsync(Guid id)
    {
        var exercicio = await _repo.GetByIdAsync(id);
        if (exercicio == null)
        {
            _logger.LogWarning("Exercício com ID {Id} não encontrado.", id);
        }
        return exercicio;
    }

    public async Task<PaginationResponse<Exercicio>> GetAllPaginatedAsync(PaginationRequest request)
    {
        var (items, totalItems) = await _repo.GetAllPaginatedAsync(request);

        return new PaginationResponse<Exercicio>(items, totalItems, request.PageNumber, request.PageSize);
    }
}
