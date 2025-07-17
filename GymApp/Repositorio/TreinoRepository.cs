using GymApp.Data;
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
    public async Task<Treino?> GetByIdAsync(Guid id)
    {
        return await _db.Treinos
            .Include(t => t.Exercicios)
                .ThenInclude(et => et.Exercicio)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
        _logger.LogInformation("Contexto salvo no banco");
    }
}
