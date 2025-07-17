using GymApp.Data;
using GymApp.Models;
using GymApp.Repositorio.Interface;

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

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
        _logger.LogInformation("Contexto salvo no banco");
    }
}
