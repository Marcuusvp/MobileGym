using GymApp.Data;
using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositorio;

public class TreinoRepository : ITreinoRepository
{
    private readonly GymAppDbContext _db;
    private readonly ILogger<TreinoRepository> _logger;

    public TreinoRepository(GymAppDbContext db, ILogger<TreinoRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Treino> AddAsync(Treino treino)
    {
        _db.Treinos.Add(treino);
        _logger.LogInformation("treino adicionado ao contexto: {Id}", treino.Id);
        return treino;
    }

    public async Task<bool> DeleteAsync(Treino treino)
    {
        _db.Treinos.Remove(treino);
        _logger.LogInformation("Treino marcado para remoção no contexto: {Id}", treino.Id);
        return true;
    }

    public async Task<Treino?> GetByIdAsync(Guid id)
    {
        return await _db.Treinos
            .Include(t => t.Exercicios)
                .ThenInclude(et => et.Exercicio)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<(IEnumerable<Treino> Items, int TotalItems)> GetTreinosByUserIdPaginatedAsync(GetTreinosDoUsuarioRequest request, string user)
    {
        var query = _db.Treinos
            .Where(t => t.Usuario == user)
            .Include(t => t.Exercicios)
                .ThenInclude(et => et.Exercicio)
            .AsQueryable();

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        _logger.LogInformation("Busca paginada de treinos para o usuário {UserId}. Página: {PageNumber}, Tamanho: {PageSize}, Total de itens: {TotalItems}",
            user, request.PageNumber, request.PageSize, totalItems);

        return (items, totalItems);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
        _logger.LogInformation("Contexto salvo no banco");
    }

    public async Task<Treino> UpdateAsync(Treino treino)
    {
        _db.Entry(treino).State = EntityState.Modified;
        _logger.LogInformation("Treino marcado para atualização no contexto: {Id}", treino.Id);
        return treino;
    }
    public async Task UpdateTreinoExerciciosAsync(Treino existingTreino, List<ExercicioTreinoUpdateDto>? exerciciosDto)
    {
        if (exerciciosDto == null || !exerciciosDto.Any())
        {
            foreach (var etToRemove in existingTreino.Exercicios.ToList())
            {
                _db.ExercicioTreinos.Remove(etToRemove);
                _logger.LogInformation("Marcado para remoção: ExercicioTreino (Id: {ExercicioTreinoId}) do treino {TreinoId} (lista vazia/nula).", etToRemove.Id, existingTreino.Id);
            }
            return;
        }

        var exerciciosIdsNaRequisicao = new HashSet<Guid>(exerciciosDto.Select(dto => dto.ExercicioId));

        var currentExercicios = existingTreino.Exercicios.ToList();
        foreach (var existingEt in currentExercicios)
        {
            if (!exerciciosIdsNaRequisicao.Contains(existingEt.ExercicioId))
            {
                _db.ExercicioTreinos.Remove(existingEt);                                                         
                _logger.LogInformation("Marcado para remoção: ExercicioTreino (Id: {ExercicioTreinoId}) para ExercicioId {ExercicioId} do treino {TreinoId}.", existingEt.Id, existingEt.ExercicioId, existingTreino.Id);
            }
        }

        foreach (var newEtDto in exerciciosDto)
        {
            var existingEtInCollection = existingTreino.Exercicios.FirstOrDefault(et => et.ExercicioId == newEtDto.ExercicioId);

            if (existingEtInCollection != null)
            {
                existingEtInCollection.Atualizar(newEtDto.Series, newEtDto.Repeticoes, newEtDto.Carga);
                _logger.LogInformation("Atualizando ExercicioTreino (Id: {ExercicioTreinoId}) para ExercicioId {ExercicioId} no treino {TreinoId}.", existingEtInCollection.Id, newEtDto.ExercicioId, existingTreino.Id);
            }
            else
            {
                var novoExercicioTreino = ExercicioTreino.Criar(
                    newEtDto.ExercicioId,
                    newEtDto.Series,
                    newEtDto.Repeticoes,
                    newEtDto.Carga
                );
                novoExercicioTreino.SetTreinoId(existingTreino.Id);
                _db.ExercicioTreinos.Add(novoExercicioTreino);
                _logger.LogInformation("Adicionando novo ExercicioTreino (Id: {ExercicioTreinoId}) para ExercicioId {ExercicioId} ao treino {TreinoId}.", novoExercicioTreino.Id, newEtDto.ExercicioId, existingTreino.Id);
            }
        }
    }
}
