using AutoMapper;
using StudyBuddy.API.Services.Mappings;

namespace StudyBuddy.API.UnitTests.TestHelpers;

public static class MapperTestHelper
{
    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        config.AssertConfigurationIsValid();
        return config.CreateMapper();
    }
}
