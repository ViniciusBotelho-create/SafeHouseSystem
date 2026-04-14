using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface ICategoryService
{
    Task CreateAsync(CreateCategoryDto dto);
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}