using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Domain.Entities;


namespace DeveloperPath.Application.Common.Mappings.Profiles
{
    public class PathProfile : SourceProfile
    {
        public PathProfile()
        {
            CreateMap<Path, PathDto>();
            CreateMap<Path, PathTitle>();
        }
    }
}
