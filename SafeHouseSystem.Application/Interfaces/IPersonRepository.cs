using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface IPersonRepository
{
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(Guid id);
    Task<Person?> GetByIdAsync(Guid id);
    Task<IEnumerable<Person>> GetAllAsync();
    Task<IEnumerable<Person>> GetAllWithTransactionsAsync();
}