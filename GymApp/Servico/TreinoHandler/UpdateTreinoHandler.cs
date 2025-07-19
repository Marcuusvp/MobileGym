using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;

namespace GymApp.Servico.TreinoHandler;

public class UpdateTreinoHandler
{
    private readonly ITreinoRepository _repo;
    private readonly ILogger<UpdateTreinoHandler> _logger;

    public UpdateTreinoHandler(ILogger<UpdateTreinoHandler> logger, ITreinoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    public async Task<Treino?> AtualizarAsync(Guid id, UpdateTreinoRequest request)
    {
        var treino = await _repo.GetByIdAsync(id);

        if (treino == null)
        {
            _logger.LogWarning("Treino com ID {Id} não encontrado para atualização.", id);
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Nome))
        {
            treino.AtualizarNome(request.Nome);
        }

        await _repo.UpdateTreinoExerciciosAsync(treino, request.Exercicios);

        await _repo.SaveChangesAsync();

        _logger.LogInformation("Treino com ID {Id} atualizado com sucesso.", treino.Id);

        return treino;
    }
}
