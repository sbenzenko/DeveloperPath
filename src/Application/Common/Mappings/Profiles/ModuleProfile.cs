using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Domain.Entities;


namespace DeveloperPath.Application.Common.Mappings.Profiles
{
    public class ModuleProfile : SourceProfile
    {
        public ModuleProfile()
        {
            CreateMap<Module, ModuleDto>();
            CreateMap<Module, ModuleTitle>();
        }
    }
}
