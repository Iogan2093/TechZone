using Microsoft.EntityFrameworkCore;
using TechZone.Data;
using TechZone.Models;

namespace TechZone.Repositories;

// Расширенный репозиторий товаров с подгрузкой связанных данных
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync();
    Task<Product?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
}

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(TechZoneDbContext context) : base(context) { }

    // Список товаров с категориями (без отслеживания — read-only сценарий)
    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
        => await _devices.Products.Include(p => p.Category).AsNoTracking().ToListAsync();

    // Один товар со всеми связанными сущностями
    public async Task<Product?> GetByIdWithDetailsAsync(int id)
        => await _devices.Products
            .Include(p => p.Category)
            .Include(p => p.Details)
            .FirstOrDefaultAsync(p => p.Id == id);

    // Фильтрация по категории
    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        => await _devices.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();
}
