using Microsoft.EntityFrameworkCore;
using TechZone.Data;
using TechZone.Models;

namespace TechZone.Services;

// Упрощённый сервис текущего пользователя (без авторизации, для демо)
public interface ICurrentUserService
{
    Task<Customer> GetOrCreateAnonBuyerAsync();
}

public class CurrentUserService : ICurrentUserService
{
    private readonly TechZoneDbContext _devices;

    // Email фиктивного покупателя демо-режима
    private const string AnonBuyerEmail = "guest@techzone.local";

    public CurrentUserService(TechZoneDbContext context) => _devices = context;

    // Возвращает существующего демо-покупателя или создаёт нового
    public async Task<Customer> GetOrCreateAnonBuyerAsync()
    {
        var customer = await _devices.Customers.FirstOrDefaultAsync(c => c.Email == AnonBuyerEmail);

        if (customer == null)
        {
            customer = new Customer
            {
                FullName = "Anon Buyer",
                Email = AnonBuyerEmail,
                RegisteredAt = DateTime.UtcNow
            };
            _devices.Customers.Add(customer);
            await _devices.SaveChangesAsync();
        }
        return customer;
    }
}
