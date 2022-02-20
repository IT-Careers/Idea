using Idea.Service.Models;

namespace Idea.Service
{
    public interface ILocationService 
    {
        LocationServiceModel CreeateLocation(LocationServiceModel locationServiceModel);

        LocationServiceModel GetLocationByCoordinate(CoordinateServiceModel coordinateServiceModel);
    }
}
