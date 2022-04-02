using AutoMapper;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles;

/// <summary>
/// AutoMapper prorfile class
/// </summary>
public class PathProfile : Profile
{
  /// <summary>
  /// AutoMapper mapping for path
  /// </summary>
  public PathProfile()
  {
    CreateMap<Domain.Entities.Path, Path>();
    CreateMap<Domain.Entities.Path, DeletedPath>();
    CreateMap<Domain.Entities.Path, PathDetails>();
    CreateMap<Domain.Entities.Path, PathTitle>();
    CreateMap<CreatePath, Domain.Entities.Path>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Modules, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.LastModified, opt => opt.Ignore())
      .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
      .ForMember(dest => dest.Deleted, opt => opt.Ignore());
    CreateMap<UpdatePath, Domain.Entities.Path>()
      .ForMember(dest => dest.Id, opt => opt.Ignore())
      .ForMember(dest => dest.Modules, opt => opt.Ignore())
      .ForMember(dest => dest.Created, opt => opt.Ignore())
      .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
      .ForMember(dest => dest.LastModified, opt => opt.Ignore())
      .ForMember(dest => dest.LastModifiedBy, opt => opt.Ignore())
      .ForMember(dest => dest.Deleted, opt => opt.Ignore());
  }
}
