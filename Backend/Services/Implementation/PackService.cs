using Backend.DAO.DTOs.Pack;
using Backend.DAO.Models;
using Backend.DAO.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class PackService(IPackRepository packRepository, IFileStorageService fileStorage, ILogger<PackService> logger)
    : IPackService
{
    private readonly IPackRepository _packRepository = packRepository;
    private readonly IFileStorageService _fileStorage = fileStorage;
    private readonly ILogger<PackService> _logger = logger;

    public async Task<IEnumerable<PackDto>> GetAllPacksAsync()
    {
        var packs = await _packRepository.GetAllAsync();
        
        return packs.Select(p => new PackDto
        {
            Id = p.Id,
            Name = p.Name,
            UrlImage = p.UrlImage
        });
    }

    public async Task<PackDto> CreatePackAsync(CreatePackDto createPackDto)
    {
        if (createPackDto.ImageFile == null)
            throw new ArgumentException("Image file is required.");

        var imageUrl = await _fileStorage.UploadFileAsync(createPackDto.ImageFile, "packs");

        var packModel = new Pack
        {
            Name = createPackDto.Name,
            UrlImage = imageUrl
        };

        var savedPack = await _packRepository.AddAsync(packModel);

        return new PackDto
        {
            Id = savedPack.Id,
            Name = savedPack.Name,
            UrlImage = savedPack.UrlImage
        };
    }
}