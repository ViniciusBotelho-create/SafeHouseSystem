using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface IPersonService
{
    Task CreateAsync(CreatePersonDto dto);
    Task<IEnumerable<PersonDto>> GetAllAsync();
    Task<PersonDto?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdatePersonDto dto);
    Task DeleteAsync(Guid id);
    Task<SummaryDto> GetSummaryAsync();
}