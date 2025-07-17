
using GymApp.Models;

namespace GymApp.Repositorio.Interface;

public interface IExercicioRepository
{
    Task<Exercicio> AddAsync(Exercicio exercicio);
    Task<Exercicio?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
}
