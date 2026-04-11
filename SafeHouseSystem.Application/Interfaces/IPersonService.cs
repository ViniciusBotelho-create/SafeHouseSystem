using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface IPersonService
{
    void Create(CreatePersonDto dto);
    IEnumerable<PersonDto> GetAll();
    PersonDto? GetById(Guid id);
    void Update(Guid id, UpdatePersonDto dto);
    void Delete(Guid id);
    SummaryDto GetSummary(); 
}