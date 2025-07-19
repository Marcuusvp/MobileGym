
using GymApp.Dto;
using GymApp.Models;

namespace GymApp.Repositorio.Interface;

public interface IExercicioRepository
{
    Task<Exercicio> AddAsync(Exercicio exercicio);
    Task<Exercicio?> GetByIdAsync(Guid id);
    Task<Exercicio> UpdateAsync(Exercicio exercicio);
    Task<bool> DeleteAsync(Exercicio exercicio);
     Task<(IEnumerable<Exercicio> Items, int TotalItems)> GetAllPaginatedAsync(PaginationRequest pagination);
    Task SaveChangesAsync();
}
