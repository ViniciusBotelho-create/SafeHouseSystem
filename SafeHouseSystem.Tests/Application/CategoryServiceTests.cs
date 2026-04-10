using FluentAssertions;
using Moq;
using Xunit;

using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Tests.Application;

public class CategoryServiceTests
{
    [Fact]
    public void Should_Call_Repository_When_Creating_Category()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repositoryMock.Object);

        var dto = new CreateCategoryDto
        {
            Name = "Food",
            Finality = CategoryFinality.Expense
        };

        // Act
        service.Create(dto);

        // Assert
        repositoryMock.Verify(r =>
            r.Add(It.Is<Category>(c =>
                c.Name == "Food" &&
                c.Finality == CategoryFinality.Expense)),
            Times.Once);
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Invalid()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repositoryMock.Object);

        var dto = new CreateCategoryDto
        {
            Name = "",
            Finality = CategoryFinality.Expense
        };

        // Act
        Action action = () => service.Create(dto);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot be empty");
    }

    [Fact]
    public void Should_Return_All_Categories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category("Food", CategoryFinality.Expense),
            new Category("Salary", CategoryFinality.Income)
        };

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetAll()).Returns(categories);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        var result = service.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result.First().Name.Should().Be("Food");
    }

    [Fact]
    public void Should_Return_Category_When_Id_Exists()
    {
        // Arrange
        var category = new Category("Food", CategoryFinality.Expense);

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetById(category.Id)).Returns(category);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        var result = service.GetById(category.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(category.Id);
        result.Name.Should().Be("Food");
    }

    [Fact]
    public void Should_Return_Null_When_Id_Does_Not_Exist()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>()))
                      .Returns((Category?)null);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        var result = service.GetById(Guid.NewGuid());

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Should_Call_Delete_When_Id_Exists()
    {
        // Arrange
        var category = new Category("Food", CategoryFinality.Expense);

        var repositoryMock = new Mock<ICategoryRepository>();
        repositoryMock.Setup(r => r.GetById(category.Id)).Returns(category);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        service.Delete(category.Id);

        // Assert
        repositoryMock.Verify(r => r.Delete(category.Id), Times.Once);
    }

    [Fact]
    public void Should_Throw_Exception_When_Deleting_Non_Existing_Category()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();

        var id = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetById(id))
                      .Returns((Category?)null);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        Action action = () => service.Delete(id);

        // Assert
        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Category not found");
    }

    [Fact]
    public void Should_Not_Call_Delete_When_Category_Does_Not_Exist()
    {
        // Arrange
        var repositoryMock = new Mock<ICategoryRepository>();
        var id = Guid.NewGuid();

        repositoryMock.Setup(r => r.GetById(id))
                      .Returns((Category?)null);

        var service = new CategoryService(repositoryMock.Object);

        // Act
        Action action = () => service.Delete(id);

        // Assert
        action.Should().Throw<ArgumentException>();
        repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }
}