namespace SafeHouseSystem.Domain.Entities;

using SafeHouseSystem.Domain.Enums;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    public Transaction(string description, decimal amount, TransactionType type)
    {
        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Type = type;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new ArgumentException("Description cannot be empty");

        if (Description.Length > 400)
            throw new ArgumentException("Description cannot exceed 400 characters");

        if (Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");
    }
}