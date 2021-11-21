using AutoMapper;
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
    }
  }
}
