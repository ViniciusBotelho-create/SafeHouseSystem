using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;

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


    public void Delete(Guid id)
    {
        var person = _repository.GetById(id);

        if (person is null)
            throw new ArgumentException("Person not found");

        _repository.Delete(id);
    }
}