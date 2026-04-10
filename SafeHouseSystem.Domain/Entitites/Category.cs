using SafeHouseSystem.Domain.Enums;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public CategoryFinality Finality { get; private set; }

    public Category(string name, CategoryFinality finality)
    {
        Id = Guid.NewGuid();
        Name = name;
        Finality = finality;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("Name cannot be empty");

        if (Name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters");

        if (!Enum.IsDefined(typeof(CategoryFinality), Finality))
            throw new ArgumentException("Invalid category finality");
    }
}