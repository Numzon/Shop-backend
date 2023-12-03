using AutoFixture.Kernel;

namespace Infrastructure.UnitTests.Extensions;
public static class FixtureExtensions
{
    public static IEnumerable<T> CreateManyWith<T>(this ISpecimenBuilder builder, int numberOfElements, params T[] with)
    {
        var collection = builder.CreateMany<T>(numberOfElements).ToList();
        collection.AddRange(with);
        return collection.AsEnumerable();
    }
}
