namespace SafeHouseSystem.Application.DTOs;

using SafeHouseSystem.Domain.Enums;

public class CreateCategoryDto
{
    public required string Description { get; set; }
    public CategoryFinality Finality { get; set; }
}