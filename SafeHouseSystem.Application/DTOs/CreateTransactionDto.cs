namespace SafeHouseSystem.Application.DTOs;

using SafeHouseSystem.Domain.Enums;

public class CreateTransactionDto
{
    public required string Description { get; set; }
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }

    public Guid CategoryId { get; set; }
    public Guid PersonId { get; set; }
}