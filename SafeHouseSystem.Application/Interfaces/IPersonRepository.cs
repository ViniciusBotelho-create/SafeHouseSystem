using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface IPersonRepository
{
    void Add(Person person);
    void Update(Person person);
    void Delete(Guid id);
    Person? GetById(Guid id);
    IEnumerable<Person> GetAll();
}