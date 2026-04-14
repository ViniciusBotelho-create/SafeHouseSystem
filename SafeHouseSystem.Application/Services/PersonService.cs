using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _repository;

    public PersonService(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(CreatePersonDto dto)
    {
        var person = new Person(dto.Name, dto.Age);
        await _repository.AddAsync(person);
    }

    public async Task<IEnumerable<PersonDto>> GetAllAsync()
    {
        var persons = await _repository.GetAllAsync();
        return persons.Select(p => new PersonDto
        {
            Id = p.Id,
            Name = p.Name,
            Age = p.Age
        });
    }

    public async Task<PersonDto?> GetByIdAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);
        if (person is null) return null;

        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }

    public async Task UpdateAsync(Guid id, UpdatePersonDto dto)
    {
        var person = await _repository.GetByIdAsync(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        person.Update(dto.Name, dto.Age);
        await _repository.UpdateAsync(person);
    }

    public async Task DeleteAsync(Guid id)
    {
        var person = await _repository.GetByIdAsync(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        await _repository.DeleteAsync(id);
    }

    public async Task<SummaryDto> GetSummaryAsync()
    {
        var persons = await _repository.GetAllWithTransactionsAsync();

        var personTotals = persons.Select(p => new PersonTotalsDto
        {
            Id = p.Id,
            Name = p.Name,
            TotalIncome = p.Transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount),
            TotalExpense = p.Transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount)
        }).ToList();

        return new SummaryDto
        {
            Persons = personTotals,
            TotalIncome = personTotals.Sum(p => p.TotalIncome),
            TotalExpense = personTotals.Sum(p => p.TotalExpense)
        };
    }
}