using Microsoft.EntityFrameworkCore;
using TechZone.Data;
using TechZone.Models;

namespace TechZone.Repositories;

// Расширенный репозиторий заказов
public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetAllWithDetailsAsync();
    Task<Order?> GetByIdWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
}

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(TechZoneDbContext context) : base(context) { }

    // Все заказы со всеми связями, новые сверху
    public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
        => await _devices.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    // Один заказ со всеми связями
    public async Task<Order?> GetByIdWithDetailsAsync(int id)
        => await _devices.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

    // Заказы конкретного покупателя
    public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId)
        => await _devices.Orders
            .Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
}
