using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Application.CQRS.Themes.Queries.GetThemes;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
    public class ThemeProfile: SourceProfile
    {
        public ThemeProfile()
        {
            CreateMap<Theme, ThemeDto>();
            CreateMap<Theme, ThemeTitle>();
        }
    }
}
