using FluentAssertions;
using Moq;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;
using Xunit;

namespace SafeHouseSystem.Tests.Application;

public class TransactionServiceTests
{
    [Fact]
    public void Should_Create_Transaction()
    {
        // Arrange
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);

        personRepoMock.Setup(r => r.GetById(person.Id)).Returns(person);
        categoryRepoMock.Setup(r => r.GetById(category.Id)).Returns(category);

        var service = new TransactionService(
            transactionRepoMock.Object,
            personRepoMock.Object,
            categoryRepoMock.Object);

        var dto = new CreateTransactionDto
        {
            Description = "Lunch",
            Amount = 50,
            Type = TransactionType.Expense,
            PersonId = person.Id,
            CategoryId = category.Id
        };

        // Act
        service.Create(dto);

        // Assert
        transactionRepoMock.Verify(r => r.Add(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public void Should_Throw_When_Person_Not_Found()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        personRepoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns((Person?)null);

        var service = new TransactionService(
            transactionRepoMock.Object,
            personRepoMock.Object,
            categoryRepoMock.Object);

        var dto = new CreateTransactionDto
        {
            Description = "Test",
            Amount = 10,
            Type = TransactionType.Expense,
            PersonId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid()
        };

        Action action = () => service.Create(dto);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Person not found");
    }

    [Fact]
    public void Should_Throw_When_Category_Not_Found()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        var person = new Person("John", 30);

        personRepoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns(person);

        categoryRepoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns((Category?)null);

        var service = new TransactionService(
            transactionRepoMock.Object,
            personRepoMock.Object,
            categoryRepoMock.Object);

        var dto = new CreateTransactionDto
        {
            Description = "Test",
            Amount = 10,
            Type = TransactionType.Expense,
            PersonId = person.Id,
            CategoryId = Guid.NewGuid()
        };

        Action action = () => service.Create(dto);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Category not found");
    }

    [Fact]
    public void Should_Return_All_Transactions()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();

        transactionRepoMock.Setup(r => r.GetAll())
            .Returns(new List<Transaction>());

        var service = new TransactionService(
            transactionRepoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        var result = service.GetAll();

        result.Should().NotBeNull();
    }

    [Fact]
    public void Should_Return_Transaction_By_Id()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateExpense(
            "Lunch",
            50,
            category,
            personId);

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetById(transaction.Id)).Returns(transaction);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        var result = service.GetById(transaction.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(transaction.Id);
    }

    [Fact]
    public void Should_Throw_When_Deleting_Non_Existing_Transaction()
    {
        var repoMock = new Mock<ITransactionRepository>();

        repoMock.Setup(r => r.GetById(It.IsAny<Guid>()))
            .Returns((Transaction?)null);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        Action action = () => service.Delete(Guid.NewGuid());

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Transaction not found");
    }

    [Fact]
    public void Should_Delete_When_Transaction_Exists()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateExpense(
            "Lunch",
            50,
            category,
            personId);

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetById(transaction.Id)).Returns(transaction);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        service.Delete(transaction.Id);

        repoMock.Verify(r => r.Delete(transaction.Id), Times.Once);
    }
}