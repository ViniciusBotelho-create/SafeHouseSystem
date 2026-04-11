using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Description { get; private set; }
    public CategoryFinality Finality { get; private set; }

    public Category(string description, CategoryFinality finality)
    {
        Id = Guid.NewGuid();
        Description = description;
        Finality = finality;

        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Description))
            throw new ArgumentException("Description cannot be empty");

        if (Description.Length > 400)
            throw new ArgumentException("Description cannot exceed 400 characters");

        if (!Enum.IsDefined(typeof(CategoryFinality), Finality))
            throw new ArgumentException("Invalid category finality");
    }
}