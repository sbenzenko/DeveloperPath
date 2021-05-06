using System;
using DeveloperPath.Domain.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Domain.UnitTests.Exceptions
{
  public class AdAccountInvalidExceptionTests
  {
    [Test]
    public void ShouldHaveCorrectMessageByDefault()
    {
      var ex = new AdAccountInvalidException("Test",new Exception());

      ex.Message.Should().Be($"AD Account \"Test\" is invalid.");
    }

    [Test]
    public void InnerExceptionShouldBeTheSameAsPassedToConstructor()
    {
      var ex = new AdAccountInvalidException("Test",new StackOverflowException());

      ex.InnerException.Should().BeEquivalentTo(new StackOverflowException());
    }
  }
}
