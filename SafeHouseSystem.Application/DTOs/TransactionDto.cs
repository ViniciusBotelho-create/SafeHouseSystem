namespace SafeHouseSystem.Application.DTOs;

using SafeHouseSystem.Domain.Enums;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }

    public Guid CategoryId { get; set; }
    public string CategoryDescription{ get; set; } = string.Empty;
}