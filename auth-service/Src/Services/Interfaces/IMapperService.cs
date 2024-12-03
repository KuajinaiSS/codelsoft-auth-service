namespace auth_service.Services.Interfaces;

/// <summary>
/// AutoMapper to map objects
/// </summary>
/// <remarks>
/// This interface and the "overuse services for everything and then only inject them" principle
/// Should looks like kind of unnecesary boilerplate, but currently 
/// .NET 8 is on their release-candidate versions and it includes a new feature called spread operator
/// so problably the implementation of this service will be changed to use that feature
/// </remarks>
public interface IMapperService
{
    public TDestination Map<TSource, TDestination>(TSource source);

    public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceItems);
}