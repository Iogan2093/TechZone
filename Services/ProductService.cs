using TechZone.Models;
using TechZone.Repositories;

namespace TechZone.Services;

// Бизнес-сервис для работы с товарами
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _stockRepo;

    public ProductService(IProductRepository repo) => _stockRepo = repo;

    public Task<IEnumerable<Product>> GetAllAsync() => _stockRepo.GetAllWithCategoryAsync();
    public Task<Product?> GetByIdAsync(int id) => _stockRepo.GetByIdWithDetailsAsync(id);
    public Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId) => _stockRepo.GetByCategoryAsync(categoryId);

    public async Task CreateAsync(Product product)
    {
        await _stockRepo.AddAsync(product);
        await _stockRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _stockRepo.Update(product);
        await _stockRepo.SaveChangesAsync();
    }

    // Удаление: сначала ищем сущность, потом удаляем
    public async Task DeleteAsync(int id)
    {
        var p = await _stockRepo.GetByIdAsync(id);
        if (p is null) return;
        _stockRepo.Remove(p);
        await _stockRepo.SaveChangesAsync();
    }
}
