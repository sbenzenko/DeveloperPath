using System.Collections.Generic;
using DeveloperPath.Domain.Common;
using FluentAssertions;
using NUnit.Framework;

namespace DeveloperPath.Domain.UnitTests.Common
{
  public class ValueObjectTests
  {
    SampleObject obj1 = new SampleObject() { Prop1 = "Test1", Prop2 = "Test2" };
    SampleObject obj2 = new SampleObject() { Prop1 = "Test1", Prop2 = "Test2" };
    SampleObject obj3 = new SampleObject() { Prop1 = "Test1", Prop2 = "Test3" };
    SampleObject obj4 = new SampleObject() { Prop1 = "Test1", Prop2 = null };
    SampleObject obj5 = new SampleObject() { Prop1 = "Test1", Prop2 = null };
    SampleObject obj6 = new SampleObject() { Prop1 = "Test1" };

    [Test]
    public void ShouldNotBeEqualToNull()
    {
      obj1.Equals(null).Should().Be(false);
    }

    [Test]
    public void ShouldNotBeEqualToOtherType()
    {
      obj1.Equals(new object()).Should().Be(false);
    }

    [Test]
    public void ShouldNotBeEqualToSelf()
    {
      obj1.Equals(obj1).Should().Be(true);
    }

    [Test]
    public void ShouldNotBeEqualToSameObject()
    {
      obj1.Equals(obj2).Should().Be(true);
    }

    [Test]
    public void ShouldNotBeEqualToOtherObject()
    {
      obj1.Equals(obj3).Should().Be(false);
    }

    [Test]
    public void ShouldNotBeEqualToObjectWithNullProperty()
    {
      obj4.Equals(obj1).Should().Be(false);
    }

    [Test]
    public void ShouldCorrectlyCompareNullProperties()
    {
      obj4.Equals(obj5).Should().Be(true);
    }

    [Test]
    public void ShouldCorrectlyCompareNullAndUnsetProperties()
    {
      obj5.Equals(obj6).Should().Be(true);
    }

    [Test]
    public void ShouldCorrectlyCountGetHashCode()
    {
      obj1.GetHashCode().Should().Be(obj1.GetHashCode());
      obj1.GetHashCode().Should().Be(obj2.GetHashCode());
      obj1.GetHashCode().Should().NotBe(obj3.GetHashCode());
    }

    [Test]
    public void ShouldCorrectlyCountGetHashCodeForNullProperties()
    {
      obj5.GetHashCode().Should().Be(obj6.GetHashCode());
    }
  }

  internal class SampleObject : ValueObject
  {
    public string Prop1 { get; init; }
    public string Prop2 { get; init; }
    protected override IEnumerable<object> GetAtomicValues()
    {
      yield return Prop1;
      yield return Prop2;
    }
  }
}
