using Microsoft.EntityFrameworkCore;
using TechZone.Data;
using TechZone.Models;
using TechZone.Repositories;

namespace TechZone.Services;

// Бизнес-сервис заказов
public interface IOrderService
{
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByCustomerAsync(int customerId);
    Task<Order> CreateFromCartAsync(int customerId, string shippingAddress);
    Task UpdateStatusAsync(int orderId, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _stockRepo;
    private readonly ICartService _bag;
    private readonly TechZoneDbContext _devices;

    public OrderService(IOrderRepository repo, ICartService cart, TechZoneDbContext context)
    {
        _stockRepo = repo;
        _bag = cart;
        _devices = context;
    }

    public Task<IEnumerable<Order>> GetAllAsync() => _stockRepo.GetAllWithDetailsAsync();
    public Task<Order?> GetByIdAsync(int id) => _stockRepo.GetByIdWithDetailsAsync(id);
    public Task<IEnumerable<Order>> GetByCustomerAsync(int customerId) => _stockRepo.GetByCustomerAsync(customerId);

    // Создаёт заказ из корзины: списывает товары, считает сумму, очищает корзину
    public async Task<Order> CreateFromCartAsync(int customerId, string shippingAddress)
    {
        var cartItems = (await _bag.GetCartAsync(customerId)).ToList();
        if (!cartItems.Any())
            throw new InvalidOperationException("Корзина пуста");

        // Цены фиксируем на момент покупки
        var order = new Order
        {
            CustomerId = customerId,
            ShippingAddress = shippingAddress,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        order.TotalAmount = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

        // Списание со склада с проверкой остатков
        foreach (var ci in cartItems)
        {
            var product = await _devices.Products.FindAsync(ci.ProductId);
            if (product != null)
            {
                if (product.Stock < ci.Quantity)
                    throw new InvalidOperationException($"Недостаточно товара '{product.Name}' на складе");
                product.Stock -= ci.Quantity;
            }
        }

        await _stockRepo.AddAsync(order);
        await _stockRepo.SaveChangesAsync();

        // После успешной покупки очищаем корзину
        await _bag.ClearCartAsync(customerId);
        return order;
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _devices.Orders.FindAsync(orderId);
        if (order is null) return;
        order.Status = status;
        await _devices.SaveChangesAsync();
    }
}
