using FluentAssertions;
using Moq;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Application.Services;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;
using Xunit;

namespace SafeHouseSystem.Tests.Application;

public class TransactionServiceTests
{
    [Fact]
    public async Task Should_Create_Transaction()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);

        personRepoMock.Setup(r => r.GetByIdAsync(person.Id)).ReturnsAsync(person);
        categoryRepoMock.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

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

        await service.CreateAsync(dto);

        transactionRepoMock.Verify(r => r.AddAsync(It.Is<Transaction>(t =>
            t.Description == "Lunch" &&
            t.Amount == 50 &&
            t.Type == TransactionType.Expense)),
            Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_Person_Not_Found()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        personRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                      .ReturnsAsync((Person?)null);

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

        Func<Task> action = async () => await service.CreateAsync(dto);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Person not found");
    }

    [Fact]
    public async Task Should_Throw_When_Category_Not_Found()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();
        var personRepoMock = new Mock<IPersonRepository>();
        var categoryRepoMock = new Mock<ICategoryRepository>();

        var person = new Person("John", 30);

        personRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                      .ReturnsAsync(person);
        categoryRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                        .ReturnsAsync((Category?)null);

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

        Func<Task> action = async () => await service.CreateAsync(dto);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Category not found");
    }

    [Fact]
    public async Task Should_Return_All_Transactions()
    {
        var transactionRepoMock = new Mock<ITransactionRepository>();

        transactionRepoMock.Setup(r => r.GetAllAsync())
                           .ReturnsAsync(new List<Transaction>());

        var service = new TransactionService(
            transactionRepoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        var result = await service.GetAllAsync();

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_Return_Transaction_By_Id()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateExpense("Lunch", 50, category, personId);

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        var result = await service.GetByIdAsync(transaction.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(transaction.Id);
    }

    [Fact]
    public async Task Should_Throw_When_Deleting_Non_Existing_Transaction()
    {
        var repoMock = new Mock<ITransactionRepository>();

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Transaction?)null);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        Func<Task> action = async () => await service.DeleteAsync(Guid.NewGuid());

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Transaction not found");
    }

    [Fact]
    public async Task Should_Delete_When_Transaction_Exists()
    {
        var category = new Category("Food", CategoryFinality.Expense);
        var personId = Guid.NewGuid();

        var transaction = Transaction.CreateExpense("Lunch", 50, category, personId);

        var repoMock = new Mock<ITransactionRepository>();
        repoMock.Setup(r => r.GetByIdAsync(transaction.Id)).ReturnsAsync(transaction);

        var service = new TransactionService(
            repoMock.Object,
            Mock.Of<IPersonRepository>(),
            Mock.Of<ICategoryRepository>());

        await service.DeleteAsync(transaction.Id);

        repoMock.Verify(r => r.DeleteAsync(transaction.Id), Times.Once);
    }
}