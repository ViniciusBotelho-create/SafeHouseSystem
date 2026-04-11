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

        var transaction = new Transaction(description, amount, type, category, Id);

        _transactions.Add(transaction);
    }
}