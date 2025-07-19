using GymApp.Data;
using GymApp.Dto;
using GymApp.Models;
using GymApp.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;

namespace GymApp.Repositorio;

public class ExercicioRepository : IExercicioRepository
{
    private readonly GymAppDbContext _db;
    private readonly ILogger<ExercicioRepository> _logger;
    public ExercicioRepository(ILogger<ExercicioRepository> logger, GymAppDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public async Task<Exercicio> AddAsync(Exercicio exercicio)
    {
        _db.Exercicios.Add(exercicio);
        _logger.LogInformation("Exercício adicionado ao contexto: {Id}", exercicio.Id);
        return exercicio;
    }

    public async Task<Exercicio?> GetByIdAsync(Guid id)
    {
        return await _db.Exercicios.FindAsync(id);
    }

    public async Task<Exercicio> UpdateAsync(Exercicio exercicio)
    {
        _db.Exercicios.Update(exercicio);
        _logger.LogInformation("Exercício atualizado no contexto: {Id}", exercicio.Id);
        return exercicio;
    }

    public async Task<bool> DeleteAsync(Exercicio exercicio)
    {
        _db.Exercicios.Remove(exercicio);
        _logger.LogInformation("Exercício marcado para remoção no contexto: {Id}", exercicio.Id);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
        _logger.LogInformation("Contexto salvo no banco");
    }

    public async Task<(IEnumerable<Exercicio> Items, int TotalItems)> GetAllPaginatedAsync(PaginationRequest pagination)
    {
        var query = _db.Exercicios.AsQueryable();

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        _logger.LogInformation("Busca paginada de exercícios. Página: {PageNumber}, Tamanho: {PageSize}, Total de itens: {TotalItems}",
            pagination.PageNumber, pagination.PageSize, totalItems);

        return (items, totalItems);
    }
}
