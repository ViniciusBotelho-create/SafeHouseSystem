using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Application.Interfaces;

public interface IPersonService
{
    void Create(CreatePersonDto dto);
    IEnumerable<PersonDto> GetAll();
    PersonDto? GetById(Guid id);
    void Delete(Guid id);
}