using Backend.DAO.Models;
using Backend.DAO.Repositories.Interfaces;

namespace Backend.DAO.Repositories.Implementations;

public class CardBaseRepository(DataContext context) : ICardBaseRepository
{
    private readonly DataContext _context = context;

    public async Task<CardBase> AddAsync(CardBase cardBase)
    {
        await _context.CardBases.AddAsync(cardBase);
        await _context.SaveChangesAsync();
        return cardBase;
    }
}