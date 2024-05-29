using Microsoft.EntityFrameworkCore;
using WebAPIWithAuth.Data;
using WebAPIWithAuth.Models;

namespace WebAPIWithAuth.Services;

public class HeroService(HeroContext context) : IHeroService
{
    private readonly HeroContext _context = context;

    public async Task<Hero?> AddAsync(HeroDTO heroDTO)
    {
        Hero hero = new()
        {
            FirstName = heroDTO.FirstName,
            LastName = heroDTO.LastName,
            IsActive = heroDTO.IsActive
        };
        _context.Hero.Add(hero);
        var result = await _context.SaveChangesAsync();
        return result >= 0 ? hero : null;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        Hero? hero = await _context.Hero.FirstOrDefaultAsync(h => h.Id == id);
        if (hero is null)
            return false;

        _context.Remove(hero);
        var result = await _context.SaveChangesAsync();
        return result >= 0;
    }

    public async Task<List<Hero>> GetAllAsync(bool? isActive)
    {
        if (isActive is null)
            return await _context.Hero.ToListAsync();
        else
            return await _context.Hero
                .Where(h => h.IsActive == isActive)
                .ToListAsync();
    }

    public async Task<Hero?> GetByIdAsync(int id) =>
        await _context.Hero.FirstOrDefaultAsync(h => h.Id == id);

    public async Task<Hero?> UpdateAsync(int id, HeroDTO heroDTO)
    {
        var hero = await _context.Hero.FirstOrDefaultAsync(h => h.Id == id);
        if (hero is null)
            return null;

        hero.FirstName = heroDTO.FirstName;
        hero.LastName = heroDTO.LastName;
        hero.IsActive = heroDTO.IsActive;

        var result = await _context.SaveChangesAsync();
        return result >= 0 ? hero : null;
    }
}
