using FluentAssertions;
using Xunit;
using SafeHouseSystem.Domain.Entities;
using SafeHouseSystem.Domain.Enums;

namespace SafeHouseSystem.Tests.Domain;

public class TransactionTests
{
    [Fact]
    public void Should_Not_Allow_Income_For_Minor()
    {
        var person = new Person("John", 15);


        Action action = () => person.AddTransaction("Salary", 100, TransactionType.Income);


        action.Should()
            .Throw<ArgumentException>()
            .WithMessage("Minors cannot have income");
    }
}