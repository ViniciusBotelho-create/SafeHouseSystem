namespace SafeHouseSystem.Application.DTOs;

public class SummaryDto
{
    public IEnumerable<PersonTotalsDto> Persons { get; set; } = [];
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance => TotalIncome - TotalExpense;
}