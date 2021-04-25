using AutoMapper;
using DeveloperPath.Domain.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
  /// <summary>
  /// AutoMapper profile class
  /// </summary>
  public class SourceProfile : Profile
  {
    /// <summary>
    /// AutoMapper mapping for module
    /// </summary>
    public SourceProfile()
    {
      CreateMap<Domain.Entities.Source, Source>();
      CreateMap<Domain.Entities.Source, SourceDetails>();
    }
  }
}
