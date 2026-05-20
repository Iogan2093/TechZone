using TechZone.Models;
using TechZone.Repositories;

namespace TechZone.Services;

// Бизнес-сервис для работы с категориями товаров
public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task CreateAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _stockRepo;

    public CategoryService(IRepository<Category> repo) => _stockRepo = repo;

    public Task<IEnumerable<Category>> GetAllAsync() => _stockRepo.GetAllAsync();
    public Task<Category?> GetByIdAsync(int id) => _stockRepo.GetByIdAsync(id);

    public async Task CreateAsync(Category category)
    {
        await _stockRepo.AddAsync(category);
        await _stockRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _stockRepo.Update(category);
        await _stockRepo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var c = await _stockRepo.GetByIdAsync(id);
        if (c is null) return;
        _stockRepo.Remove(c);
        await _stockRepo.SaveChangesAsync();
    }
}
