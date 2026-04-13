namespace SafeHouseSystem.Application.DTOs;

public class CategoryTotalsDto
{
    public Guid CategoryId { get; set; }
    public string CategoryDescription { get; set; } = null!;
    public decimal Total { get; set; }
}