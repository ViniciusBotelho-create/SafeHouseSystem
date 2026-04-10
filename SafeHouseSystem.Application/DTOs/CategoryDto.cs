namespace SafeHouseSystem.Application.DTOs;

using SafeHouseSystem.Domain.Enums;

public class CategoryDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public CategoryFinality Finality { get; set; }
}