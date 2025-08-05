using Backend.DAO.DTOs.CardBase;

namespace Backend.Services.Interfaces;

public interface ICardBaseService
{
    Task<int> CreateCardBaseAsync(CreateCardBaseDto dto);
}