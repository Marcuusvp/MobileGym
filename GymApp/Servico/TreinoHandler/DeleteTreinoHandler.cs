using GymApp.Repositorio.Interface;

namespace GymApp.Servico.TreinoHandler;

public class DeleteTreinoHandler
{
    private readonly ITreinoRepository _repo;
    private readonly ILogger<DeleteTreinoHandler> _logger;

    public DeleteTreinoHandler(ILogger<DeleteTreinoHandler> logger, ITreinoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }
    public async Task<bool> DeletarAsync(Guid id)
    {
        var treino = await _repo.GetByIdAsync(id);

        if (treino == null)
        {
            _logger.LogWarning("Treino com ID {Id} não encontrado para deleção.", id);
            return false;
        }

        // Se o Treino tivesse imagens ou outros recursos externos,
        // a lógica para deletá-los iria aqui, similar ao Exercicio.

        await _repo.DeleteAsync(treino);
        await _repo.SaveChangesAsync();

        _logger.LogInformation("Treino com ID {Id} deletado com sucesso.", treino.Id);

        return true;
    }
}
