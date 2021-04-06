﻿using Microsoft.AspNetCore.Mvc;

namespace DeveloperPath.Web.WebAPI
{
  public class TestBase
  {
    protected T GetObjectResultContent<T>(ActionResult<T> result)
    {
      return (T)((ObjectResult)result.Result).Value;
    }
  }
}
