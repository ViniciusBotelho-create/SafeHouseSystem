using SafeHouseSystem.Application.DTOs;

namespace SafeHouseSystem.Application.Interfaces;

public interface ICategoryService
{
    void Create(CreateCategoryDto dto);
    IEnumerable<CategoryDto> GetAll();
    CategoryDto? GetById(Guid id);
    void Delete(Guid id);
}