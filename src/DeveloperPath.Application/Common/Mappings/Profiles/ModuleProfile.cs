using AutoMapper;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles;

/// <summary>
/// AutoMapper mapping profile
/// </summary>
public class ModuleProfile : Profile
{
  /// <summary>
  /// AutoMapper mapping for module
  /// </summary>
  public ModuleProfile()
  {
    CreateMap<Domain.Entities.Module, Module>()
      .ForMember(dest => dest.ShowDetails, opt => opt.Ignore());
    CreateMap<Domain.Entities.Module, ModuleDetails>();
    CreateMap<Domain.Entities.Module, ModuleTitle>();
    CreateMap<UpdateModule, Domain.Entities.Module>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Paths, opt => opt.Ignore())
      .ForMember(dest => dest.Sections, opt => opt.Ignore())
      .ForMember(dest => dest.Themes, opt => opt.Ignore())
      .ForMember(dest => dest.Prerequisites, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.LastModified, opt => opt.Ignore())
      .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
      .ForMember(dest => dest.Deleted, opt => opt.Ignore());
  }
}
