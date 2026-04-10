using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface ICategoryRepository
{
    void Add(Category category);
    void Delete(Guid id);
    Category? GetById(Guid id);
    IEnumerable<Category> GetAll();
}