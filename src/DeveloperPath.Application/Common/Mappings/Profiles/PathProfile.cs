using AutoMapper;

using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles;

/// <summary>
/// AutoMapper profile class
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
      .ForMember(x => x.IsVisible, expression => expression.Ignore())
      .ForMember(x => x.Modules, expression => expression.Ignore())
      .ForMember(x => x.Deleted, expression => expression.Ignore())
      .ForMember(x => x.Id, expression => expression.Ignore())
      .ForMember(x => x.CreatedBy, expression => expression.Ignore())
      .ForMember(x => x.Created, expression => expression.Ignore())
      .ForMember(x => x.LastModifiedBy, expression => expression.Ignore())
      .ForMember(x => x.LastModified, expression => expression.Ignore());

    CreateMap<UpdatePath, Domain.Entities.Path>()
      .ForMember(x => x.Modules, expression => expression.Ignore())
      .ForMember(x => x.Deleted, expression => expression.Ignore())
      .ForMember(x => x.CreatedBy, expression => expression.Ignore())
      .ForMember(x => x.Created, expression => expression.Ignore())
      .ForMember(x => x.LastModifiedBy, expression => expression.Ignore())
      .ForMember(x => x.LastModified, expression => expression.Ignore()); ;
  }
}