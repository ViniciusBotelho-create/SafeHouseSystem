using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface ITransactionService
{
    void Create(CreateTransactionDto dto);

    IEnumerable<TransactionDto> GetAll();
    TransactionDto? GetById(Guid id);

    void Delete(Guid id);
}