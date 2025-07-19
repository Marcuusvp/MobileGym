using GymApp.Dto;
using GymApp.Models;

namespace GymApp.Repositorio.Interface;

public interface ITreinoRepository
{
    Task<Treino> AddAsync(Treino treino);
    Task<Treino?> GetByIdAsync(Guid id);
    Task SaveChangesAsync();
    Task<Treino> UpdateAsync(Treino treino);
    Task<bool> DeleteAsync(Treino treino);
    Task UpdateTreinoExerciciosAsync(Treino treino, List<ExercicioTreinoUpdateDto>? exerciciosDto);
    Task<(IEnumerable<Treino> Items, int TotalItems)> GetTreinosByUserIdPaginatedAsync(GetTreinosDoUsuarioRequest request);
}
