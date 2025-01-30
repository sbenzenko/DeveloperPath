using DeveloperPath.WebApi.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NUnit.Framework;

namespace Web.WebAPI.Controllers;

public class TestAuthControllerTests : TestBase
{
  public TestAuthControllerTests()
  {
  }

  [Test]
  public void Get_ReturnsOk()
  {
    var controller = new TestAuthController();

    var result = controller.Get();
    var okResult = result as OkObjectResult;

    Assert.That(okResult, Is.Not.Null);
    Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    Assert.That(okResult.Value, Is.EqualTo("access allowed"));
  }
}
