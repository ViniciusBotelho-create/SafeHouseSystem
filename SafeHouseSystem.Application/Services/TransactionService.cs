using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;

namespace SafeHouseSystem.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IPersonRepository _personRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IPersonRepository personRepository,
        ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _personRepository = personRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task CreateAsync(CreateTransactionDto dto)
    {
        var person = await _personRepository.GetByIdAsync(dto.PersonId);
        if (person is null)
            throw new ArgumentException("Person not found");

        var category = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (category is null)
            throw new ArgumentException("Category not found");

        var transaction = person.AddTransaction(dto.Description, dto.Amount, dto.Type, category);
        await _transactionRepository.AddAsync(transaction);
    }

    public async Task<IEnumerable<TransactionDto>> GetAllAsync()
    {
        var transactions = await _transactionRepository.GetAllAsync();
        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            Description = t.Description,
            Amount = t.Amount,
            Type = t.Type,
            CategoryId = t.Category.Id,
            CategoryDescription = t.Category.Description
        });
    }

    public async Task<TransactionDto?> GetByIdAsync(Guid id)
    {
        var t = await _transactionRepository.GetByIdAsync(id);
        if (t is null) return null;

        return new TransactionDto
        {
            Id = t.Id,
            Description = t.Description,
            Amount = t.Amount,
            Type = t.Type,
            CategoryId = t.Category.Id,
            CategoryDescription = t.Category.Description
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id);

        if (transaction is null)
            throw new ArgumentException("Transaction not found");

        await _transactionRepository.DeleteAsync(id);
    }
}