using AutoMapper;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
    /// <summary>
    /// AutoMapper profile class
    /// </summary>
    public class ThemeProfile : Profile
  {
    /// <summary>
    /// AutoMapper mapping for theme
    /// </summary>
    public ThemeProfile()
    {
      CreateMap<Domain.Entities.Theme, Theme>();
      CreateMap<Domain.Entities.Theme, ThemeDetails>();
      CreateMap<Domain.Entities.Theme, ThemeTitle>();
    }
  }
}
