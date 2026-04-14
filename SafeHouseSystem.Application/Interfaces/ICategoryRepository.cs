using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task DeleteAsync(Guid id);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
}