using DeveloperPath.Domain.Exceptions;
using DeveloperPath.Domain.ValueObjects;

using NUnit.Framework;

namespace DeveloperPath.Domain.UnitTests.ValueObjects;

public class AdAccountTests
{
  [Test]
  public void ShouldHaveCorrectDomainAndName()
  {
    const string accountString = "SSW\\Jason";

    var account = AdAccount.For(accountString);

    Assert.That(account.Domain, Is.EqualTo("SSW"));
    Assert.That(account.Name, Is.EqualTo("Jason"));
  }

  [Test]
  public void ToStringReturnsCorrectFormat()
  {
    const string accountString = "SSW\\Jason";

    var account = AdAccount.For(accountString);

    var result = account.ToString();

    Assert.That(result, Is.EqualTo(accountString));
  }

  [Test]
  public void ImplicitConversionToStringResultsInCorrectString()
  {
    const string accountString = "SSW\\Jason";

    var account = AdAccount.For(accountString);

    string result = account;

    Assert.That(result, Is.EqualTo(accountString));
  }

  [Test]
  public void ExplicitConversionFromStringSetsDomainAndName()
  {
    const string accountString = "SSW\\Jason";

    var account = (AdAccount)accountString;

    Assert.That(account.Domain, Is.EqualTo("SSW"));
    Assert.That(account.Name, Is.EqualTo("Jason"));
  }

  [Test]
  public void ShouldThrowAdAccountInvalidExceptionForInvalidAdAccount()
  {
    Assert.Throws<AdAccountInvalidException>(() => AdAccount.For("SSWJason"));
  }
}