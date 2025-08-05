using Backend.DAO.Models;
using Backend.DAO.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.DAO.Repositories.Implementations;

public class PackRepository(DataContext context) : IPackRepository
{
    private readonly DataContext _context = context;

    public async Task<IEnumerable<Pack>> GetAllAsync()
    {
        return await _context.Packs.AsNoTracking().ToListAsync();
    }

    public async Task<Pack> AddAsync(Pack pack)
    {
        await _context.Packs.AddAsync(pack);
        await _context.SaveChangesAsync();
        return pack;
    }
}