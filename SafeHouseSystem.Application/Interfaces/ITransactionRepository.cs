using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface ITransactionRepository
{
    void Add(Transaction transaction);
    void Delete(Guid id);

    Transaction? GetById(Guid id);
    IEnumerable<Transaction> GetAll();
}