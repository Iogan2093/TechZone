using Microsoft.EntityFrameworkCore;
using TechZone.Models;

namespace TechZone.Data;

// Стартовые данные TechZone — магазин электроники
public static class DataInitializer
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Смартфоны", Description = "Современные мобильные устройства" },
            new Category { Id = 2, Name = "Ноутбуки", Description = "Ультрабуки и игровые ноутбуки" },
            new Category { Id = 3, Name = "Аксессуары", Description = "Наушники, клавиатуры, кабели" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "iPhone 15 Pro", Description = "Apple A17 Pro, 256 ГБ", Price = 119990m, Stock = 8, CategoryId = 1, ImageUrl = "https://placehold.co/400x400/0a2540/00d4ff?text=iPhone+15", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 2, Name = "Samsung Galaxy S24 Ultra", Description = "Snapdragon 8 Gen 3, 512 ГБ", Price = 134990m, Stock = 5, CategoryId = 1, ImageUrl = "https://placehold.co/400x400/0a2540/00d4ff?text=Galaxy+S24", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 3, Name = "MacBook Pro 14", Description = "Apple M3 Pro, 18 ГБ ОЗУ", Price = 249990m, Stock = 4, CategoryId = 2, ImageUrl = "https://placehold.co/400x400/0a2540/00d4ff?text=MacBook+Pro", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 4, Name = "ASUS ROG Strix G16", Description = "RTX 4070, 32 ГБ ОЗУ", Price = 189990m, Stock = 6, CategoryId = 2, ImageUrl = "https://placehold.co/400x400/1a3a5c/00d4ff?text=ROG+Strix", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 5, Name = "AirPods Pro 2", Description = "Активное шумоподавление", Price = 24990m, Stock = 30, CategoryId = 3, ImageUrl = "https://placehold.co/400x400/00d4ff/0a2540?text=AirPods+Pro", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Product { Id = 6, Name = "Logitech MX Keys", Description = "Беспроводная клавиатура", Price = 12990m, Stock = 25, CategoryId = 3, ImageUrl = "https://placehold.co/400x400/00d4ff/0a2540?text=MX+Keys", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<ProductDetails>().HasData(
            new ProductDetails { Id = 1, ProductId = 1, Manufacturer = "Apple", CountryOfOrigin = "Китай", Weight = 0.187, Dimensions = "146.6x70.6x8.25 мм", WarrantyMonths = 12 },
            new ProductDetails { Id = 2, ProductId = 3, Manufacturer = "Apple", CountryOfOrigin = "Китай", Weight = 1.55, Dimensions = "31.26x22.12x1.55 см", WarrantyMonths = 12 },
            new ProductDetails { Id = 3, ProductId = 5, Manufacturer = "Apple", CountryOfOrigin = "Вьетнам", Weight = 0.05, Dimensions = "60.6x21.8x24 мм", WarrantyMonths = 12 }
        );

        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, FullName = "Алексей Гаджетов", Email = "alex@techzone.local", Phone = "+7-911-000-00-02", Address = "Москва, Кутузовский пр., 15", RegisteredAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
    }
}
