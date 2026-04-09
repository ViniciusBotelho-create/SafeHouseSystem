using FluentAssertions;
using Xunit;
using SafeHouseSystem.Domain.Entities;

namespace SafeHouseSystem.Tests.Domain;

public class PersonTests
{
    [Fact]
    public void Should_Create_Valid_Person()
    {

        var name = "John";
        var age = 30;


        var person = new Person(name, age);


        person.Name.Should().Be(name);
        person.Age.Should().Be(age);
        person.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Is_Empty()
    {

        Action action = () => new Person("", 25);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot be empty");
    }

    [Fact]
    public void Should_Throw_Exception_When_Name_Exceeds_Max_Length()
    {

        var longName = new string('a', 201);

        Action action = () => new Person(longName, 25);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Name cannot exceed 200 characters");
    }

    [Fact]
    public void Should_Throw_Exception_When_Age_Is_Negative()
    {

        Action action = () => new Person("John", -1);

        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Age cannot be negative");
    }

    [Fact]
    public void Should_Generate_Different_Ids_For_Persons_With_Same_Name()
    {
        var name = "John";


        var person1 = new Person(name, 25);
        var person2 = new Person(name, 30);

        person1.Id.Should().NotBe(person2.Id);
    }
}