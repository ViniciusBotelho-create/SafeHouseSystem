using FluentAssertions;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;
using System.Transactions;
using Xunit;

// To not conflict with System.Transactions.Transaction, we can alias our Transaction class
using Transaction = SafeHouseSystem.Domain.Entities.Transaction;

namespace SafeHouseSystem.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Should_Create_Valid_Expense_Transaction()
    {

        var description = "Food";
        var amount = 50;


        var transaction = new Transaction(description, amount, TransactionType.Expense);


        transaction.Description.Should().Be(description);
        transaction.Amount.Should().Be(amount);
        transaction.Type.Should().Be(TransactionType.Expense);
    }

    [Fact]
    public void Should_Throw_Exception_When_Amount_Is_Negative()
    {
        Action action = () => new Transaction("Food", -10, TransactionType.Expense);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Amount must be greater than zero");
    }

    [Fact]
    public void Should_Throw_Exception_When_Description_Is_Empty()
    {
        Action action = () => new Transaction("", 10, TransactionType.Expense);

        action.Should().Throw<ArgumentException>()
            .WithMessage("Description cannot be empty");
    }
    [Fact]
    public void Should_Not_Allow_Expense_With_Income_Category()
    {

        var person = new Person("John", 30);
        var category = new Category("Salary", CategoryFinality.Income);


        Action action = () => person.AddTransaction("Test", 100, TransactionType.Expense, category);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid category for expense");
    }

    [Fact]
    public void Should_Not_Allow_Income_With_Expense_Category()
    {

        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);


        Action action = () => person.AddTransaction("Test", 100, TransactionType.Income, category);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Invalid category for income");
    }

    [Fact]
    public void Should_Allow_Transaction_With_Compatible_Category()
    {

        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);


        var action = () => person.AddTransaction("Lunch", 50, TransactionType.Expense, category);

        action.Should().NotThrow();
    }

    [Fact]
    public void Should_Allow_Transaction_With_Both_Category()
    {

        var person = new Person("John", 30);
        var category = new Category("Investment", CategoryFinality.Both);


        var action = () => person.AddTransaction("Test", 100, TransactionType.Income, category);


        action.Should().NotThrow();
    }

    [Fact]
    public void Should_Not_Allow_Income_For_Minor_Even_With_Valid_Category()
    {

        var person = new Person("John", 15);
        var category = new Category("Salary", CategoryFinality.Income);


        Action action = () => person.AddTransaction("Test", 100, TransactionType.Income, category);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Minors cannot have income");
    }
}