using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Domain.Entities;

public class Person
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Age { get; private set; }

    private readonly List<Transaction> _transactions = new();
    public IReadOnlyCollection<Transaction> Transactions => _transactions;

    public Person(string name, int age)
    {
        Id = Guid.NewGuid();
        Name = name;
        Age = age;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name cannot be empty");

        if (Name.Length > 200)
            throw new ArgumentException("Name cannot exceed 200 characters");

        if (Age < 0)
            throw new ArgumentException("Age cannot be negative");
    }

    public void AddTransaction(string description, decimal amount, TransactionType type, Category category)
    {

        if (Age < 18 && type == TransactionType.Income)
            throw new ArgumentException("Minors cannot have income");


        if (type == TransactionType.Expense && category.Finality == CategoryFinality.Income)
            throw new ArgumentException("Invalid category for expense");

        if (type == TransactionType.Income && category.Finality == CategoryFinality.Expense)
            throw new ArgumentException("Invalid category for income");


        var transaction = new Transaction(description, amount, type);

        _transactions.Add(transaction);
    }
}