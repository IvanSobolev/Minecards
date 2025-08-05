using Backend.DAO.Models;

namespace Backend.DAO.Repositories.Interfaces;

public interface ICardBaseRepository
{
    Task<CardBase> AddAsync(CardBase cardBase);
}