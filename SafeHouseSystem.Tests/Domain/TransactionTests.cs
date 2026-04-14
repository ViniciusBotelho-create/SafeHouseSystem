using FluentAssertions;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;
using Xunit;

using Transaction = SafeHouseSystem.Domain.Entities.Transaction;

namespace SafeHouseSystem.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Should_Create_Valid_Expense_Transaction()
    {
        var description = "Food";
        var amount = 50;
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateExpense(description, amount, category, personId);

        transaction.Description.Should().Be(description);
        transaction.Amount.Should().Be(amount);
        transaction.Type.Should().Be(TransactionType.Expense);
    }

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Negative()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        Action action = () =>
            Transaction.CreateExpense("Food", -10, category, personId);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Amount must be greater than zero");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Empty()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        Action action = () =>
            Transaction.CreateExpense("", 10, category, personId);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be empty");
    }

    [Fact]
    public void Should_Not_Allow_Expense_With_Income_Category()
    {
        var person = new Person("John", 30);
        var category = new Category("Salary", CategoryFinality.Income);

        Action action = () =>
            person.AddTransaction("Test", 100, TransactionType.Expense, category);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid category for expense");
    }

    [Fact]
    public void Should_Not_Allow_Income_With_Expense_Category()
    {
        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);

        Action action = () =>
            person.AddTransaction("Test", 100, TransactionType.Income, category);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid category for income");
    }

    [Fact]
    public void Should_Allow_Transaction_With_Compatible_Category()
    {
        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);

        Action action = () =>
            person.AddTransaction("Lunch", 50, TransactionType.Expense, category);

        action.Should().NotThrow();
    }

    [Fact]
    public void Should_Allow_Transaction_With_Both_Category()
    {
        var person = new Person("John", 30);
        var category = new Category("Investment", CategoryFinality.Both);

        Action action = () =>
            person.AddTransaction("Test", 100, TransactionType.Income, category);

        action.Should().NotThrow();
    }

    [Fact]
    public void Should_Throw_When_Description_Too_Long()
    {
        var longDescription = new string('a', 401);
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        Action action = () =>
            Transaction.CreateExpense(longDescription, 10, category, personId);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Description cannot exceed 400 characters");
    }

    [Fact]
    public void Should_Throw_When_Amount_Is_Zero()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        Action action = () =>
            Transaction.CreateExpense("Food", 0, category, personId);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Amount must be greater than zero");
    }

    [Fact]
    public void Should_Create_Valid_Income_Transaction()
    {
        var category = new Category("Salary", CategoryFinality.Income);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateIncome("Salary", 3000, category, personId);

        transaction.Type.Should().Be(TransactionType.Income);
        transaction.Amount.Should().Be(3000);
    }
}