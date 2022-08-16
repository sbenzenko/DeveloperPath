using AutoMapper;
using DeveloperPath.Application.CQRS.Modules.Commands.UpdateModule;
using DeveloperPath.Shared.ClientModels;

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
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
              .ForMember(x=>x.ShowDetails, expression => expression.Ignore());
            CreateMap<Domain.Entities.Module, ModuleDetails>();
            CreateMap<Domain.Entities.Module, ModuleTitle>();
            CreateMap<UpdateModule, Domain.Entities.Module>()
              .ForMember(x=>x.Id, expression => expression.Ignore())
              .ForMember(x => x.Paths, expression => expression.Ignore())
              .ForMember(x => x.Deleted, expression => expression.Ignore())
              .ForMember(x => x.Sections, expression => expression.Ignore())
              .ForMember(x => x.Themes, expression => expression.Ignore())
              .ForMember(x => x.Prerequisites, expression => expression.Ignore())
              .ForMember(x => x.Created, expression => expression.Ignore())
              .ForMember(x => x.CreatedBy, expression => expression.Ignore())
              .ForMember(x => x.LastModified, expression => expression.Ignore())
              .ForMember(x => x.LastModifiedBy, expression => expression.Ignore());
        }
    }
}
