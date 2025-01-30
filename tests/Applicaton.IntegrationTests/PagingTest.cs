using DeveloperPath.WebApi.Models;

using NUnit.Framework;

namespace DeveloperPath.Application.IntegrationTests
{
  public class PagingTest
  {
    [Test]
    [TestCase(0, 5)]
    [TestCase(2, 5)]
    [TestCase(5, 5)]
    [TestCase(6, 10)]
    [TestCase(10, 10)]
    [TestCase(11, 25)]
    [TestCase(24, 25)]
    [TestCase(25, 25)]
    [TestCase(26, 50)]
    [TestCase(49, 50)]
    [TestCase(50, 50)]
    [TestCase(75, 100)]
    [TestCase(101, 100)]
    [TestCase(1223234401, 100)]
    public void TestPaging(int request, int response)
    {
      RequestParams rq = new RequestParams()
      {
        PageSize = request
      };

      Assert.That(rq.PageSize, Is.EqualTo(response));
    }
  }
}
