using FluentAssertions;
using Moq;
using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Application.Services;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Tests.Application;

public class CategoryServiceTests
{
    [Fact]
    public async Task Should_Call_Repository_When_Creating_Category()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repositoryMock.Object);

        var dto = new CreateCategoryDto
        {
            Description = "Food",
            Finality = CategoryFinality.Expense
        };

        await service.CreateAsync(dto);

        repositoryMock.Verify(r =>
            r.AddAsync(It.Is<Category>(c =>
                c.Description == "Food" &&
                c.Finality == CategoryFinality.Expense)),
            Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Description_Is_Invalid()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repositoryMock.Object);

        var dto = new CreateCategoryDto
        {
            Description = "",
            Finality = CategoryFinality.Expense
        };

        Func<Task> action = async () => await service.CreateAsync(dto);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Description cannot be empty");
    }

    [Fact]
    public async Task Should_Return_All_Categories()
    {
        var categories = new List<Category>
        {
            new Category("Food", CategoryFinality.Expense),
            new Category("Salary", CategoryFinality.Income)
        };

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

        var service = new CategoryService(repositoryMock.Object);

        var result = await service.GetAllAsync();

        result.Should().HaveCount(2);
        result.First().Description.Should().Be("Food");
    }

    [Fact]
    public async Task Should_Return_Category_When_Id_Exists()
    {
        var category = new Category("Food", CategoryFinality.Expense);

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var service = new CategoryService(repositoryMock.Object);

        var result = await service.GetByIdAsync(category.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
        result.Description.Should().Be("Food");
    }

    [Fact]
    public async Task Should_Return_Null_When_Id_Does_Not_Exist()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                      .ReturnsAsync((Category?)null);

        var service = new CategoryService(repositoryMock.Object);

        var result = await service.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_Call_Delete_When_Id_Exists()
    {
        var category = new Category("Food", CategoryFinality.Expense);

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetByIdAsync(category.Id)).ReturnsAsync(category);

        var service = new CategoryService(repositoryMock.Object);

        await service.DeleteAsync(category.Id);

        repositoryMock.Verify(r => r.DeleteAsync(category.Id), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_Exception_When_Deleting_Non_Existing_Category()
    {
        var repositoryMock = new Mock<ICategoryRepository>();
        var id = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetByIdAsync(id))
                      .ReturnsAsync((Category?)null);

        var service = new CategoryService(repositoryMock.Object);

        Func<Task> action = async () => await service.DeleteAsync(id);

        await action.Should()
            .ThrowAsync<ArgumentException>()
            .WithMessage("Category not found");
    }
}