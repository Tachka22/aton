
using aton.domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace aton.infrastructure.Data;

public static class SeedData
{
    public async static Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        await context.Database.MigrateAsync();
        
        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                Guid = Guid.NewGuid(),
                Login = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin123"),
                Name = "Администратор",
                Gender = 1,
                Birthday = new DateTime(1980, 1, 1).ToUniversalTime(),
                Admin = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                ModifiedOn = DateTime.UtcNow,
                ModifiedBy = "System"
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
