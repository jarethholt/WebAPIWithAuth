using WebAPIWithAuth.Models;

namespace WebAPIWithAuth.Services;

public interface IHeroService
{
    Task<List<Hero>> GetAllAsync(bool? isActive);

    Task<Hero?> GetByIdAsync(int id);

    Task<Hero?> AddAsync(HeroDTO heroDTO);

    Task<Hero?> UpdateAsync(int id, HeroDTO heroDTO);

    Task<bool> DeleteByIdAsync(int id);
}
