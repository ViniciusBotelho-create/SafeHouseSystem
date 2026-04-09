using FluentAssertions;
using Xunit;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Tests.Domain;

public class CategoryTests
{
    [Fact]
    public void Should_Create_Valid_Category()
    {
        // Arrange
        var description = "Food";

        // Act
        var category = new Category(description, CategoryFinality.Expense);

        // Assert
        category.Description.Should().Be(description);
        category.Finality.Should().Be(CategoryFinality.Expense);
        category.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Empty()
    {
        Action action = () => new Category("", CategoryFinality.Expense);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Description cannot be empty");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Exceeds_Max_Length()
    {
        var longDescription = new string('a', 401);

        Action action = () => new Category(longDescription, CategoryFinality.Expense);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Description cannot exceed 400 characters");
    }
}