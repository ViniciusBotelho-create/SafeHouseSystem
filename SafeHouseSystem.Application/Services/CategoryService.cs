using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public void Create(CreateCategoryDto dto)
    {
        var category = new Category(dto.Description, dto.Finality);
        _repository.Add(category);
    }

    public IEnumerable<CategoryDto> GetAll()
    {
        return _repository.GetAll()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Description = c.Description,
                Finality = c.Finality
            });
    }

    public CategoryDto? GetById(Guid id)
    {
        var category = _repository.GetById(id);

        if (category is null)
            return null;

        return new CategoryDto
        {
            Id = category.Id,
            Description = category.Description,
            Finality = category.Finality
        };
    }

    public void Delete(Guid id)
    {
        var category = _repository.GetById(id);

        if (category is null)
            throw new ArgumentException("Category not found");

        _repository.Delete(id);
    }
}