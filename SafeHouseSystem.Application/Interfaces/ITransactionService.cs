using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface ITransactionService
{
    Task CreateAsync(CreateTransactionDto dto);
    Task<IEnumerable<TransactionDto>> GetAllAsync();
    Task<TransactionDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}