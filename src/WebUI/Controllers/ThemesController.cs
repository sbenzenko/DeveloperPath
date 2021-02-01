using DeveloperPath.Application.Common.Models;
using DeveloperPath.Application.Themes.Commands.CreateTheme;
using DeveloperPath.Application.Themes.Commands.DeleteTheme;
using DeveloperPath.Application.Themes.Commands.UpdateTheme;
using DeveloperPath.Application.Themes.Queries.GetThemes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeveloperPath.WebUI.Controllers
{
  //[Authorize]
  [Route("api/modules/{moduleId}/themes")]
  public class ThemesController : ApiController
  {
    /// <summary>
    /// Get theme information by its Id
    /// </summary>
    /// <param name="moduleId">An id of the theme</param>
    /// <param name="themeId">An id of the theme</param>
    /// <returns>Detailed information of the theme with sources included</returns>
    [HttpGet("{themeId}", Name = "GetTheme")]
    [HttpHead("{themeId}")]
    public async Task<ActionResult<ThemeViewModel>> Get(int moduleId, int themeId)
    {
      ThemeViewModel model = await Mediator.Send(new GetThemeQuery { Id = themeId, ModuleId = moduleId });

      return Ok(model);
    }

    /// <summary>
    /// Create a theme
    /// </summary>
    /// <param name="command">Theme object</param>
    /// <returns>An Id of created theme</returns>
    [HttpPost]
    public async Task<ActionResult<ThemeDto>> Create(CreateThemeCommand command)
    {
      ThemeDto model = await Mediator.Send(command);

      return CreatedAtRoute("GetTheme", new { moduleId = model.ModuleId, themeId = model.Id }, model);
    }

    /// <summary>
    /// Update the theme with given Id
    /// </summary>
    /// <param name="themeId">An id of the theme</param>
    /// <param name="command">Updated theme object</param>
    /// <returns></returns>
    [HttpPut("{themeId}")]
    public async Task<ActionResult<ThemeDto>> Update(int themeId, UpdateThemeCommand command)
    {
      if (themeId != command.Id)
      {
        return BadRequest();
      }

      return Ok(await Mediator.Send(command));
    }

    /// <summary>
    /// Delete the theme with given Id
    /// </summary>
    /// <param name="moduleId">And id of the module the theme is in</param>
    /// <param name="themeId">An id of the theme</param>
    /// <returns></returns>
    [HttpDelete("{themeId}")]
    public async Task<ActionResult> Delete(int moduleId, int themeId)
    {
      await Mediator.Send(new DeleteThemeCommand { Id = themeId, ModuleId = moduleId });

      return NoContent();
    }
  }
}
