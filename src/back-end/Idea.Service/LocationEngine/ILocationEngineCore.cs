using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;

namespace Idea.Service.LocationEngine
{
    internal interface ILocationEngineCore
    {
        List<Location> GetLocationsForCoordinate(Coordinate coordinate);

        Location GenerateLocation(Coordinate coordinate);
    }
}
