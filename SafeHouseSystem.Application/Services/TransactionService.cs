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

    public void Create(CreateTransactionDto dto)
    {
        var person = _personRepository.GetById(dto.PersonId);
        if (person is null)
            throw new ArgumentException("Person not found");

        var category = _categoryRepository.GetById(dto.CategoryId);
        if (category is null)
            throw new ArgumentException("Category not found");


        person.AddTransaction(dto.Description, dto.Amount, dto.Type, category);


        var transaction = person.Transactions.Last();

        _transactionRepository.Add(transaction);
    }

    public IEnumerable<TransactionDto> GetAll()
    {
        return _transactionRepository.GetAll()
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Type = t.Type,
                CategoryId = t.Category.Id,
                CategoryDescription = t.Category.Description
            });
    }

    public TransactionDto? GetById(Guid id)
    {
        var t = _transactionRepository.GetById(id);
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

    public IEnumerable<CategoryTotalsDto> GetTotalsByCategory()
    {
        return _transactionRepository.GetTotalsByCategory()
            .Select(x => new CategoryTotalsDto
            {
                CategoryId = x.CategoryId,
                CategoryDescription = x.CategoryDescription,
                Total = x.Total
            });
    }

    public IEnumerable<CategoryTotalsDto> GetTotalsByCategoryId(Guid categoryId)
    {
        var category = _categoryRepository.GetById(categoryId);
        if (category is null)
            throw new ArgumentException("Category not found");

        return _transactionRepository.GetTotalsByCategoryId(categoryId)
            .Select(x => new CategoryTotalsDto
            {
                CategoryId = x.CategoryId,
                CategoryDescription = x.CategoryDescription,
                Total = x.Total
            });
    }

    public void Delete(Guid id)
    {
        var transaction = _transactionRepository.GetById(id);

        if (transaction is null)
            throw new ArgumentException("Transaction not found");

        _transactionRepository.Delete(id);
    }
}