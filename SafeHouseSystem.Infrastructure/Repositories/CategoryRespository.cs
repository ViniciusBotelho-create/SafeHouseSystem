using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Infrastructure.Data;

namespace SafeHouseSystem.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var category = _context.Categories.Find(id);

        if (category is null)
            return;

        _context.Categories.Remove(category);
        _context.SaveChanges();
    }

    public Category? GetById(Guid id)
    {
        return _context.Categories.Find(id);
    }

    public IEnumerable<Category> GetAll()
    {
        return _context.Categories.ToList();
    }
}