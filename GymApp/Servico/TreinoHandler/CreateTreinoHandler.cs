using GymApp.Data;
using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;

namespace GymApp.Servico.TreinoHandler;

public class CreateTreinoHandler
{
    private readonly ILogger<CreateTreinoHandler> _logger;
    private readonly ITreinoRepository _repo;

    public CreateTreinoHandler(ILogger<CreateTreinoHandler> logger, ITreinoRepository repo)
    {
        _logger = logger;
        _repo = repo;
    }

    public async Task<Treino> CriarAsync(CreateTreinoRequest request)
    {
        var exerciciosDoTreino = request.Exercicios.Select(e => ExercicioTreino.Criar(
            e.ExercicioId,
            e.Series,
            e.Repeticoes,
            e.Carga
        )).ToList();

        var treino = Treino.Criar(
            request.Nome,
            exerciciosDoTreino,
            request.Usuario);

        await _repo.AddAsync(treino);

        await _repo.SaveChangesAsync();

        _logger.LogInformation("Treino criado com ID {Id}", treino.Id);
        return treino;
    }

    public async Task<Treino?> BuscarTreinoPorId(Guid id)
    {
        var treino = await _repo.GetByIdAsync(id);
        return treino;
    }
}
