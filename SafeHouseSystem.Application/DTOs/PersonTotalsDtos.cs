namespace SafeHouseSystem.Application.DTOs;

public class PersonTotalsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}