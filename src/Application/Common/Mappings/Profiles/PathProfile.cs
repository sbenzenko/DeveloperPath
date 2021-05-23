using AutoMapper;
using DeveloperPath.Application.CQRS.Paths.Commands.CreatePath;
using DeveloperPath.Application.CQRS.Paths.Commands.UpdatePath;
using DeveloperPath.Domain.Shared.ClientModels;
using Shared.ClientModels;


namespace DeveloperPath.Application.Common.Mappings.Profiles
{
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
            CreateMap<Domain.Entities.Path, Path>();
            CreateMap<Domain.Entities.Path, DeletedPath>();
            CreateMap<Domain.Entities.Path, PathDetails>();
            CreateMap<Domain.Entities.Path, PathTitle>();
            CreateMap<CreatePath, Domain.Entities.Path > ();

            CreateMap<UpdatePath, Domain.Entities.Path>()
                .ForMember(x => x.Id, expression => expression.Ignore());
        }
    }
}
