using AutoMapper;
using DeveloperPath.Domain.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
  /// <summary>
  /// AutoMapper profile class
  /// </summary>
  public class SectionProfile : Profile
  {
    /// <summary>
    /// AutoMapper mapping for section
    /// </summary>
    public SectionProfile()
    {
      CreateMap<Domain.Entities.Section, Section>();
    }
  }
}
