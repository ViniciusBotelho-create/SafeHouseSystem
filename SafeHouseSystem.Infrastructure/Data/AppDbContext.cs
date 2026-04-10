using Microsoft.EntityFrameworkCore;
using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }

    public DbSet<Category> Categories { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}