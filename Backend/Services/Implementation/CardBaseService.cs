using Backend.DAO.DTOs.CardBase;
using Backend.DAO.Models;
using Backend.DAO.Repositories.Interfaces;
using Backend.Services.Interfaces;

namespace Backend.Services.Implementation;

public class CardBaseService(ICardBaseRepository cardBaseRepository, IFileStorageService fileStorage)
    : ICardBaseService
{
    private readonly ICardBaseRepository _cardBaseRepository = cardBaseRepository;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<int> CreateCardBaseAsync(CreateCardBaseDto dto)
    {
        var skinUrl = await _fileStorage.UploadFileAsync(dto.SkinFile, "skins");
        var backgroundUrl = await _fileStorage.UploadFileAsync(dto.BackgroundImageFile, "backgrounds");

        var cardBaseModel = new CardBase
        {
            Name = dto.Name,
            Creator = dto.Creator,
            BaseRarityLevel = dto.BaseRarityLevel,
            SkinUrl = skinUrl,
            CardBgPhotoUrl = backgroundUrl,
            PackId = dto.PackId
        };

        var savedCardBase = await _cardBaseRepository.AddAsync(cardBaseModel);

        return savedCardBase.Id;
    }
}