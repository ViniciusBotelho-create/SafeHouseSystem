using FluentAssertions;
using Moq;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Application.Services;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Tests.Application;

public class PersonServiceTests
{
    [Fact]
    public async Task Should_Call_Repository_When_Creating_Person()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var dto = new CreatePersonDto
        {
            Name = "John",
            Age = 30
        };

        await service.CreateAsync(dto);

        repositoryMock.Verify(r =>
            r.AddAsync(It.Is<Person>(p => p.Name == "John" && p.Age == 30)),
            Times.Once);
    }

    [Fact]
    public async Task Should_Return_All_Persons_As_Dto()
    {
        var persons = new List<Person>
        {
            new Person("John", 30),
            new Person("Maria", 25)
        };

        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(persons);

        var service = new PersonService(repositoryMock.Object);

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p is PersonDto);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Name_Is_Invalid()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var dto = new CreatePersonDto
        {
            Name = "",
            Age = 30
        };

        Func<Task> action = async () => await service.CreateAsync(dto);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Name cannot be empty");
    }

    [Fact]
    public async Task Should_Return_PersonDto_When_Id_Exists()
    {
        var person = new Person("John", 30);

        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(person.Id)).ReturnsAsync(person);

        var service = new PersonService(repositoryMock.Object);

        var result = await service.GetByIdAsync(person.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(person.Id);
        result.Name.Should().Be("John");
        result.Age.Should().Be(30);
    }

    [Fact]
    public async Task Should_Return_Null_When_Id_Does_Not_Exist()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                      .ReturnsAsync((Person?)null);

        var service = new PersonService(repositoryMock.Object);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Call_Delete_When_Id_Exists()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var id = Guid.NewGuid();
        var person = new Person("John", 30);

        repositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(person);

        await service.DeleteAsync(id);

        repositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Deleting_Non_Existing_Person()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        var id = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetByIdAsync(id))
                      .ReturnsAsync((Person?)null);

        var service = new PersonService(repositoryMock.Object);

        Func<Task> action = async () => await service.DeleteAsync(id);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Person not found");
    }

    [Fact]
    public async Task Should_Not_Call_Delete_When_Person_Does_Not_Exist()
    {
        var repositoryMock = new Mock<IPersonRepository>();
        var id = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetByIdAsync(id))
                      .ReturnsAsync((Person?)null);

        var service = new PersonService(repositoryMock.Object);

        Func<Task> action = async () => await service.DeleteAsync(id);

        await action.Should().ThrowAsync<ArgumentException>();
        repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task Should_Return_Summary_With_Correct_Totals()
    {
        var person = new Person("John", 30);
        var category = new Category("Food", CategoryFinality.Expense);
        person.AddTransaction("Lunch", 100, TransactionType.Expense, category);
        person.AddTransaction("Dinner", 50, TransactionType.Expense, category);

        var repoMock = new Mock<IPersonRepository>();
        repoMock.Setup(r => r.GetAllWithTransactionsAsync())
                .ReturnsAsync(new List<Person> { person });

        var service = new PersonService(repoMock.Object);
        var summary = await service.GetSummaryAsync();

        summary.TotalExpense.Should().Be(150);
        summary.TotalIncome.Should().Be(0);
        summary.Persons.Should().HaveCount(1);
    }
}