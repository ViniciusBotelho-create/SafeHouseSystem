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

        var name = "Food";


        var category = new Category(name, CategoryFinality.Expense);


        category.Name.Should().Be(name);
        category.Finality.Should().Be(CategoryFinality.Expense);
        category.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Empty()
    {
        Action action = () => new Category("", CategoryFinality.Expense);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot be empty");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Exceeds_Max_Length()
    {
        var longName = new string('a', 101);

        Action action = () => new Category(longName, CategoryFinality.Expense);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 100 characters");
    }
}