using GymApp.Models;

namespace GymApp.Repositorio.Interface;

public interface ITreinoRepository
{
    Task<Treino> AddAsync(Treino treino);
    Task<Treino?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
