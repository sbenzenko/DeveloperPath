using System.Collections.Generic;

using DeveloperPath.Domain.Common;

using NUnit.Framework;

namespace DeveloperPath.Domain.UnitTests.Common;

public class ValueObjectTests
{
  readonly SampleObject obj1 = new() { Prop1 = "Test1", Prop2 = "Test2" };
  readonly SampleObject obj2 = new() { Prop1 = "Test1", Prop2 = "Test2" };
  readonly SampleObject obj3 = new() { Prop1 = "Test1", Prop2 = "Test3" };
  readonly SampleObject obj4 = new() { Prop1 = "Test1", Prop2 = null };
  readonly SampleObject obj5 = new() { Prop1 = "Test1", Prop2 = null };
  readonly SampleObject obj6 = new() { Prop1 = "Test1" };

  [Test]
  public void ShouldNotBeEqualToNull()
  {
    Assert.That(obj1.Equals(null), Is.False);
  }

  [Test]
  public void ShouldNotBeEqualToOtherType()
  {
    Assert.That(obj1.Equals(new object()), Is.False);
  }

  [Test]
  public void ShouldNotBeEqualToSelf()
  {
    Assert.That(obj1.Equals(obj1), Is.True);
  }

  [Test]
  public void ShouldNotBeEqualToSameObject()
  {
    Assert.That(obj1.Equals(obj2), Is.True);
  }

  [Test]
  public void ShouldNotBeEqualToOtherObject()
  {
    Assert.That(obj1.Equals(obj3), Is.False);
  }

  [Test]
  public void ShouldNotBeEqualToObjectWithNullProperty()
  {
    Assert.That(obj4.Equals(obj1), Is.False);
  }

  [Test]
  public void ShouldCorrectlyCompareNullProperties()
  {
    Assert.That(obj4.Equals(obj5), Is.True);
  }

  [Test]
  public void ShouldCorrectlyCompareNullAndUnsetProperties()
  {
    Assert.That(obj5.Equals(obj6), Is.True);
  }

  [Test]
  public void ShouldCorrectlyCountGetHashCode()
  {
    Assert.That(obj1.GetHashCode(), Is.EqualTo(obj1.GetHashCode()));
    Assert.That(obj1.GetHashCode(), Is.EqualTo(obj2.GetHashCode()));
    Assert.That(obj1.GetHashCode(), Is.Not.EqualTo(obj3.GetHashCode()));
  }

  [Test]
  public void ShouldCorrectlyCountGetHashCodeForNullProperties()
  {
    Assert.That(obj1.GetHashCode(), Is.Not.EqualTo(obj6.GetHashCode()));
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