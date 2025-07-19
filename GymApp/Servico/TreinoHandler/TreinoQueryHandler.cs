
using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;

namespace GymApp.Servico.TreinoHandler;

public class TreinoQueryHandler
{
    private readonly ITreinoRepository _repo;
    private readonly ILogger<TreinoQueryHandler> _logger;

    public TreinoQueryHandler(ILogger<TreinoQueryHandler> logger, ITreinoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    public TreinoQueryHandler(ITreinoRepository repo, ILogger<TreinoQueryHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Treino?> GetByIdAsync(Guid id)
    {
        var treino = await _repo.GetByIdAsync(id);
        if (treino == null)
        {
            _logger.LogWarning("Treino com ID {Id} não encontrado.", id);
        }
        return treino;
    }

    public async Task<PaginationResponse<GetTreinoResponse>> GetTreinosByUserIdPaginatedAsync(GetTreinosDoUsuarioRequest request)
    {
        var (treinos, totalItems) = await _repo.GetTreinosByUserIdPaginatedAsync(request);

        // Mapeia os treinos para o DTO de resposta
        var treinoDtos = treinos.Select(treino => new GetTreinoResponse
        {
            Id = treino.Id,
            Nome = treino.Nome,
            Exercicios = treino.Exercicios.Select(et => new ExercicioDoTreinoDto
            {
                ExercicioId = et.ExercicioId,
                Series = et.Series,
                Repeticoes = et.Repeticoes,
                Carga = et.Carga
            }).ToList()
        }).ToList();

        return new PaginationResponse<GetTreinoResponse>(treinoDtos, totalItems, request.PageNumber, request.PageSize);
    }
}
