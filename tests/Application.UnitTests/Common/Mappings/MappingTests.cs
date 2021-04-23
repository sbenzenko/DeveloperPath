using AutoMapper;
using DeveloperPath.Application.Common.Mappings.Interfaces;
using DeveloperPath.Application.Common.Mappings.Profiles;
using DeveloperPath.Application.CQRS.Modules.Queries.GetModules;
using DeveloperPath.Application.CQRS.Paths.Queries.GetPaths;
using DeveloperPath.Domain.Entities;
using NUnit.Framework;
using System;

namespace DeveloperPath.Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.AddProfile<ModuleProfile>();
                cfg.AddProfile<PathProfile>();
                cfg.AddProfile<SectionProfile>();
                cfg.AddProfile<SourceProfile>();
                cfg.AddProfile<ThemeProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid<MappingProfile>();
        }
        
        [Test]
        [TestCase(typeof(Path), typeof(PathViewModel))]
        [TestCase(typeof(Module), typeof(ModuleViewModel))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
