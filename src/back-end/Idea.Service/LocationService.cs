using Idea.Data;
using Idea.Data.Models.Locations;
using Idea.Service.LocationEngine;
using Idea.Service.Mapping;
using Idea.Service.Models;

namespace Idea.Service
{
    public class LocationService : ILocationService
    {
        private readonly IdeaDbContext ideaDbContext;
        private readonly ILocationEngineCore locationEngine;

        public LocationService(IdeaDbContext ideaDbContext)
        {
            this.ideaDbContext = ideaDbContext;
            this.locationEngine = new LocationEngineCore(ideaDbContext);
        }

        public async Task<LocationServiceModel> CreeateLocation(LocationServiceModel locationServiceModel)
        {
            Location created = (await this.ideaDbContext.AddAsync(locationServiceModel.ToEntity())).Entity;

            return created.ToServiceModel();
        }

        public async Task<LocationServiceModel> GetLocationByCoordinate(CoordinateServiceModel coordinateServiceModel)
        {
            return null;
        }
    }
}
