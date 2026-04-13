using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface ITransactionRepository
{
    void Add(Transaction transaction);
    void Delete(Guid id);

    Transaction? GetById(Guid id);
    IEnumerable<Transaction> GetAll();

    IEnumerable<(Guid CategoryId, string CategoryDescription, decimal Total)> GetTotalsByCategory();

    IEnumerable<(Guid CategoryId, string CategoryDescription, decimal Total)> GetTotalsByCategoryId(Guid categoryId);
}