namespace SafeHouseSystem.Application.DTOs;

using SafeHouseSystem.Domain.Enums;

public class CreateCategoryDto
{
    public required string Name { get; set; }
    public CategoryFinality Finality { get; set; }
}