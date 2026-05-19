using AutoMapper;
using StudyBuddy.API.Services.Mappings;
using Microsoft.Extensions.Logging.Abstractions; 

namespace StudyBuddy.API.UnitTests.TestHelpers;

public static class MapperTestHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        }, NullLoggerFactory.Instance);

        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }
}
