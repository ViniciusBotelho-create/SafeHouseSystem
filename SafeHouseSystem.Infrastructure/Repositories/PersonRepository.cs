using Microsoft.EntityFrameworkCore;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Infrastructure.Data;

namespace SafeHouseSystem.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Person person)
    {
        _context.Persons.Add(person);
        _context.SaveChanges();
    }

    public void Update(Person person)
    {
        _context.Persons.Update(person);
        _context.SaveChanges();
    }


    public void Delete(Guid id)
    {
        var person = _context.Persons.Find(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        _context.Persons.Remove(person);
        _context.SaveChanges();
    }

    public Person? GetById(Guid id)
    {
        return _context.Persons.Find(id);
    }

    public IEnumerable<Person> GetAll()
    {
        return _context.Persons.ToList();
    }
}