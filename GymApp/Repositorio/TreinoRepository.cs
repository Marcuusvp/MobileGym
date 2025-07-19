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

    public async Task<(IEnumerable<Treino> Items, int TotalItems)> GetTreinosByUserIdPaginatedAsync(GetTreinosDoUsuarioRequest request)
    {
        var query = _db.Treinos
            .Where(t => t.Usuario == request.Usuario)
            .Include(t => t.Exercicios) // Incluir exercícios se necessário no retorno da lista
                .ThenInclude(et => et.Exercicio) // Incluir detalhes do exercício
            .AsQueryable();

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        _logger.LogInformation("Busca paginada de treinos para o usuário {UserId}. Página: {PageNumber}, Tamanho: {PageSize}, Total de itens: {TotalItems}",
            request.Usuario, request.PageNumber, request.PageSize, totalItems);

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
        if (exerciciosDto == null)
        {
            _logger.LogInformation("Lista de exercícios nula para o treino {TreinoId}. Nenhuma atualização de exercícios será realizada.", existingTreino.Id);
            return;
        }
        
        // IDs dos exercícios que devem existir no treino APÓS a atualização
        var exercicioIdsNaRequisicao = new HashSet<Guid>(exerciciosDto.Select(dto => dto.ExercicioId));
        // 1. REMOVER: Itens que estão no banco de dados, mas não estão na requisição
        // Filtra os ExercicioTreino existentes que não têm um ExercicioId correspondente na nova lista
        // 1. REMOVER: Itens que estão no banco de dados, mas não estão na requisição
        var exerciciosParaRemover = existingTreino.Exercicios
                                            .Where(et => !exercicioIdsNaRequisicao.Contains(et.ExercicioId))
                                            .ToList();

        foreach (var et in exerciciosParaRemover)
        {
            _db.ExercicioTreinos.Remove(et); // Marca para remoção no DB
            // existingTreino.Exercicios.Remove(et); // Esta linha pode ser mantida ou removida, dependendo da sua preferência de manter a coleção em memória sincronizada imediatamente. O EF Core vai cuidar da persistência.
            _logger.LogInformation("Removendo ExercicioTreino para ExercicioId {ExercicioId} do treino {TreinoId}.", et.ExercicioId, existingTreino.Id);
        }
        // 2. ADICIONAR ou ATUALIZAR: Itens que estão na requisição
        foreach (var newEtDto in exerciciosDto)
        {
            // Tenta encontrar um ExercicioTreino existente para este ExercicioId no treino
            var existingEt = existingTreino.Exercicios.FirstOrDefault(et => et.ExercicioId == newEtDto.ExercicioId);

            if (existingEt != null)
            {
                // ATUALIZAR: Se já existe, atualiza as propriedades do item rastreado
                existingEt.Atualizar(newEtDto.Series, newEtDto.Repeticoes, newEtDto.Carga);
                _db.Entry(existingEt).State = EntityState.Modified; // Garante que é marcado como modificado
                _logger.LogInformation("Atualizando ExercicioTreino para ExercicioId {ExercicioId} no treino {TreinoId}.", newEtDto.ExercicioId, existingTreino.Id);
            }
            else
            {
                // ADICIONAR: Se não existe, cria um novo ExercicioTreino e adiciona à coleção rastreada
                var novoExercicioTreino = ExercicioTreino.Criar(
                    newEtDto.ExercicioId,
                    newEtDto.Series,
                    newEtDto.Repeticoes,
                    newEtDto.Carga
                );
                existingTreino.Exercicios.Add(novoExercicioTreino); // Adiciona ao treino existente (rastreado)
                _logger.LogInformation("Adicionando novo ExercicioTreino para ExercicioId {ExercicioId} ao treino {TreinoId}.", newEtDto.ExercicioId, existingTreino.Id);
            }
        }
    }
}
