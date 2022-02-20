using Idea.Service.Models;

namespace Idea.Service
{
    public interface ILocationService 
    {
        Task<LocationServiceModel> CreeateLocation(LocationServiceModel locationServiceModel);

        Task<LocationServiceModel> GetLocationByCoordinate(CoordinateServiceModel coordinateServiceModel);
    }
}
