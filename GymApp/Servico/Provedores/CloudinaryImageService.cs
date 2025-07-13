
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GymApp.Dto;

namespace GymApp.Servico.Provedores;

public class CloudinaryImageService : IImageStorageService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryImageService> _logger;

    public CloudinaryImageService(
        Cloudinary cloudinary,
        ILogger<CloudinaryImageService> logger)
    {
        _logger = logger;
        _cloudinary = cloudinary;
    }

    public async Task<NewImageDto> UploadImageAsync(IFormFile imageFile, string folder = "exercicios")
    {
        try
        {
            await using var stream = imageFile.OpenReadStream();

            var uploadsParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                UseFilename = false,
                Overwrite = true,
                Folder = "exercicios"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadsParams);
            if (uploadResult.Error != null)
            {
                _logger.LogError("Erro ao fazer upload no Cloudinary: {Message}", uploadResult.Error.Message);
                throw new Exception("Erro ao fazer upload da imagem no Cloudinary.");
            }
            return new NewImageDto
            {
                PublicId = uploadResult.PublicId,
                ImageUrl = uploadResult.SecureUrl.ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Erro ao fazer upload no Cloudinary: {Message}", ex.Message);
            throw new Exception("Erro ao fazer upload da imagem no Cloudinary.");
        }

    }

    public async Task<bool> DeleteImageAsync(string publicImageId)
    {
        var deleteParams = new DeletionParams(publicImageId);
        var result = await _cloudinary.DestroyAsync(deleteParams);

        if (result.Error != null)
        {
            _logger.LogError("Erro ao deletar no Cloudinary: {Message}", result.Error.Message);
            return false;
        }
        return true;
    }
}
