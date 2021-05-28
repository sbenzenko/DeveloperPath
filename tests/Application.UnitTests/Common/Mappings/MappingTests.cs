using System;
using AutoMapper;
using NUnit.Framework;
using DeveloperPath.Application.Common.Mappings.Interfaces;
using DeveloperPath.Application.Common.Mappings.Profiles;
using DeveloperPath.Domain.Shared.ClientModels;
using Shared.ClientModels;

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
                cfg.AddProfile<PathProfile>();
                cfg.AddProfile<ModuleProfile>();
                cfg.AddProfile<SectionProfile>();
                cfg.AddProfile<ThemeProfile>();
                cfg.AddProfile<SourceProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid<MappingProfile>();
        }
        
        [Test]
        [TestCase(typeof(Domain.Entities.Path), typeof(Path))]
        [TestCase(typeof(Domain.Entities.Path), typeof(PathDetails))]
        [TestCase(typeof(Domain.Entities.Path), typeof(PathTitle))]
        [TestCase(typeof(Domain.Entities.Module), typeof(Module))]
        [TestCase(typeof(Domain.Entities.Module), typeof(ModuleDetails))]
        [TestCase(typeof(Domain.Entities.Module), typeof(ModuleTitle))]
        [TestCase(typeof(Domain.Entities.Section), typeof(Section))]
        [TestCase(typeof(Domain.Entities.Theme), typeof(Theme))]
        [TestCase(typeof(Domain.Entities.Theme), typeof(ThemeDetails))]
        [TestCase(typeof(Domain.Entities.Theme), typeof(ThemeTitle))]
        [TestCase(typeof(Domain.Entities.Source), typeof(Source))]
        [TestCase(typeof(Domain.Entities.Source), typeof(SourceDetails))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
