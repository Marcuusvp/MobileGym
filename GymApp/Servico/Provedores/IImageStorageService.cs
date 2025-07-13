using GymApp.Dto;

namespace GymApp.Servico.Provedores;

public interface IImageStorageService
{
    Task<NewImageDto> UploadImageAsync(IFormFile imageFile, string folder = "exercicios");
    Task<bool> DeleteImageAsync(string publicImageId);
}
