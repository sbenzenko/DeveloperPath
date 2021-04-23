using Application.Shared.Dtos.Models;
using AutoMapper;
using DeveloperPath.Domain.Entities;
 

namespace DeveloperPath.Application.Common.Mappings.Profiles
{
    public class SourceProfile: Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceDto>();
        }
    }
}
