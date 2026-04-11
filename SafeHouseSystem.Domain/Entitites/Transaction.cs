namespace SafeHouseSystem.Domain.Entities;

using SafeHouseSystem.Domain.Enums;

public class Transaction
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = null!;
    public decimal Amount { get; private set; }
    public TransactionType Type { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    public Guid PersonId { get; private set; }

    private Transaction() { }

    public Transaction(string description, decimal amount, TransactionType type, Category category, Guid personId)
    {
        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Type = type;

        Category = category;
        CategoryId = category.Id;

        PersonId = personId;

        Validate();
        ValidateCategoryCompatibility();
    }
    private void ValidateCategoryCompatibility()
    {
        if (Category is null)
            throw new ArgumentException("Category cannot be null");

        if (Type == TransactionType.Expense && Category.Finality == CategoryFinality.Income)
            throw new ArgumentException("Invalid category for expense");

        if (Type == TransactionType.Income && Category.Finality == CategoryFinality.Expense)
            throw new ArgumentException("Invalid category for income");
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new ArgumentException("Description cannot be empty");

        if (Description.Length > 400)
            throw new ArgumentException("Description cannot exceed 400 characters");

        if (Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        if (!Enum.IsDefined(typeof(TransactionType), Type))
            throw new ArgumentException("Invalid transaction type");

    }

    public static Transaction CreateExpense(
        string description,
        decimal amount,
        Category category,
        Guid personId)
    {
        return new Transaction(description, amount, TransactionType.Expense, category, personId);
    }

    public static Transaction CreateIncome(
        string description,
        decimal amount,
        Category category,
        Guid personId)
    {
        return new Transaction(description, amount, TransactionType.Income, category, personId);
    }
}