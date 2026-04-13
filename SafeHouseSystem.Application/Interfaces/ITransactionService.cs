using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface ITransactionService
{
    void Create(CreateTransactionDto dto);

    IEnumerable<TransactionDto> GetAll();
    TransactionDto? GetById(Guid id);

    IEnumerable<CategoryTotalsDto> GetTotalsByCategory();

    IEnumerable<CategoryTotalsDto> GetTotalsByCategoryId(Guid categoryId);
    void Delete(Guid id);
}