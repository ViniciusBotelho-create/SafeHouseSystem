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

    public void Create(CreatePersonDto dto)
    {
        var person = new Person(dto.Name, dto.Age);
        _repository.Add(person);
    }

    public IEnumerable<PersonDto> GetAll()
    {
        return _repository.GetAll()
            .Select(p => new PersonDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age
            });
    }

    public PersonDto? GetById(Guid id)
    {
        var person = _repository.GetById(id);
        if (person is null) return null;

        return new PersonDto
        {
            Id = person.Id,
            Name = person.Name,
            Age = person.Age
        };
    }

    public void Update(Guid id, UpdatePersonDto dto)
    {
        var person = _repository.GetById(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        person.Update(dto.Name, dto.Age);
        _repository.Update(person);
    }

    public void Delete(Guid id)
    {
        var person = _repository.GetById(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        _repository.Delete(id);
    }

    public SummaryDto GetSummary()
    {
        var persons = _repository.GetAllWithTransactions();

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