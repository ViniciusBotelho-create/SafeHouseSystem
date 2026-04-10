
using FluentAssertions;
using Moq;

using SafeHouseSystem.Application.DTOs;
using SafeHouseSystem.Application.Interfaces;
using SafeHouseSystem.Domain.Entities;


namespace SafeHouseSystem.Tests.Application;

public class PersonServiceTests
{
    [Fact]
    public void Should_Call_Repository_When_Creating_Person()
    {

        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var dto = new CreatePersonDto
        {
            Name = "John",
            Age = 30
        };


        service.Create(dto);


        repositoryMock.Verify(r =>  
            r.Add(It.Is<Person>(p => p.Name == "John" && p.Age == 30)),
            Times.Once);
    }

    [Fact]
    public void Should_Return_All_Persons_As_Dto()
    {

        var persons = new List<Person>
    {
        new Person("John", 30),
        new Person("Maria", 25)
    };

        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetAll()).Returns(persons);

        var service = new PersonService(repositoryMock.Object);


        var result = service.GetAll();


        result.Should().HaveCount(2);
        result.Should().OnlyContain(p => p is PersonDto);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Invalid()
    {

        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var dto = new CreatePersonDto
        {
            Name = "",
            Age = 30
        };


        Action action = () => service.Create(dto);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot be empty");
    }

    [Fact]
    public void Should_Return_PersonDto_When_Id_Exists()
    {

        var person = new Person("John", 30);

        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetById(person.Id)).Returns(person);

        var service = new PersonService(repositoryMock.Object);


        var result = service.GetById(person.Id);


        result.Should().NotBeNull();
        result!.Id.Should().Be(person.Id);
        result.Name.Should().Be("John");
        result.Age.Should().Be(30);
    }
    [Fact]
    public void Should_Return_Null_When_Id_Does_Not_Exist()
    {

        var repositoryMock = new Mock<IPersonRepository>();
        repositoryMock.Setup(r => r.GetById(It.IsAny<Guid>())).Returns((Person?)null);

        var service = new PersonService(repositoryMock.Object);

        var result = service.GetById(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public void Should_Call_Delete_When_Id_Exists()
    {

        var repositoryMock = new Mock<IPersonRepository>();
        var service = new PersonService(repositoryMock.Object);

        var id = Guid.NewGuid();
        var person = new Person("John", 30);

        repositoryMock
            .Setup(r => r.GetById(id))
            .Returns(person);


        service.Delete(id);


        repositoryMock.Verify(r => r.Delete(id), Times.Once);
    }
    [Fact]
    public void Should_Throw_Exception_When_Deleting_Non_Existing_Person()
    {

        var repositoryMock = new Mock<IPersonRepository>();

        var id = Guid.NewGuid();

        repositoryMock
            .Setup(r => r.GetById(id))
            .Returns((Person?)null);

        var service = new PersonService(repositoryMock.Object);


        Action action = () => service.Delete(id);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Person not found");
    }

    [Fact]
    public void Should_Not_Call_Delete_When_Person_Does_Not_Exist()
    {

        var repositoryMock = new Mock<IPersonRepository>();
        var id = Guid.NewGuid();

        repositoryMock
            .Setup(r => r.GetById(id))
            .Returns((Person?)null);

        var service = new PersonService(repositoryMock.Object);


        Action action = () => service.Delete(id);


        action.Should().Throw<ArgumentException>();
        repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
    }
}