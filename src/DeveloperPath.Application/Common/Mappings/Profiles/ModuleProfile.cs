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
            CreateMap<Domain.Entities.Module, Module>();
            CreateMap<Domain.Entities.Module, ModuleDetails>();
            CreateMap<Domain.Entities.Module, ModuleTitle>();
            CreateMap<UpdateModule, Domain.Entities.Module>()
                .ForMember(x=>x.Id, expression => expression.Ignore());
        }
    }
}
