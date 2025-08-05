using Backend.DAO.Models;

namespace Backend.DAO.Repositories.Interfaces;

public interface IPackRepository
{
    Task<IEnumerable<Pack>> GetAllAsync();
    Task<Pack> AddAsync(Pack pack);
}