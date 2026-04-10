using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Infrastructure.Repositories;

public class InMemoryPersonRepository : IPersonRepository
{
    private readonly List<Person> _persons = new();

    public void Add(Person person)
    {
        _persons.Add(person);
    }

    public void Update(Person person)
    {
        // simplificado
    }

    public void Delete(Guid id)
    {
        var person = _persons.FirstOrDefault(p => p.Id == id);
        if (person != null)
            _persons.Remove(person);
    }

    public Person? GetById(Guid id)
    {
        return _persons.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<Person> GetAll()
    {
        return _persons;
    }
}