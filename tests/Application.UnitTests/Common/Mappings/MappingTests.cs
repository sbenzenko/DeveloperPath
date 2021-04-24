using AutoMapper;
using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Application.Modules.Queries.GetModules;
using DeveloperPath.Application.Paths.Queries.GetPaths;
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
            });

            _mapper = _configuration.CreateMapper();
        }

        [Test]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }
        
        [Test]
        [TestCase(typeof(Path), typeof(PathDetails))]
        [TestCase(typeof(Module), typeof(ModuleDetails))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = Activator.CreateInstance(source);

            _mapper.Map(instance, source, destination);
        }
    }
}
