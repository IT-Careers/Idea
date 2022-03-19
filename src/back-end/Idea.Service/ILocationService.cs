using Idea.Data.Models.Shared;
using Idea.Service.Models;

namespace Idea.Service
{
    public interface ILocationService 
    {
        Task<List<LocationServiceModel>> GetAllLocations();

        Task<LocationServiceModel> CreeateLocation(LocationServiceModel locationServiceModel);

        Task<List<LocationServiceModel>> GetLocationsByCoordinate(CoordinateServiceModel coordinateServiceModel);

        Task GenerateLocationsIfNecessary(Coordinate shipCoordinate);
    }
}
