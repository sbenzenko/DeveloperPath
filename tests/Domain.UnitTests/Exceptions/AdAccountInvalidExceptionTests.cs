using System;

using DeveloperPath.Domain.Exceptions;

using NUnit.Framework;

namespace DeveloperPath.Domain.UnitTests.Exceptions;

public class AdAccountInvalidExceptionTests
{
  [Test]
  public void ShouldHaveCorrectMessageByDefault()
  {
    var ex = new AdAccountInvalidException("Test", new Exception());

    Assert.That(ex.Message, Is.EqualTo($"AD Account \"Test\" is invalid."));
  }

  [Test]
  public void InnerExceptionShouldBeTheSameAsPassedToConstructor()
  {
    var ex = new AdAccountInvalidException("Test", new StackOverflowException());

    Assert.That(ex.InnerException, Is.TypeOf<StackOverflowException>());
  }
}