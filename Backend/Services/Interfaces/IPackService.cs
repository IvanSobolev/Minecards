using Backend.DAO.DTOs.Pack;

namespace Backend.Services.Interfaces;

public interface IPackService
{
    Task<IEnumerable<PackDto>> GetAllPacksAsync();
    Task<PackDto> CreatePackAsync(CreatePackDto createPackDto);
}