using Idea.Data;
using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;
using Idea.Service.LocationEngine;
using Idea.Service.Mapping;
using Idea.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Idea.Service
{
    public class LocationService : ILocationService
    {
        private readonly IdeaDbContext ideaDbContext;
        private readonly IRandomService randomService;
        private readonly ILocationEngineCore locationEngine;

        public LocationService(IdeaDbContext ideaDbContext, IRandomService randomService)
        {
            this.ideaDbContext = ideaDbContext;
            this.locationEngine = new LocationEngineCore(ideaDbContext, randomService);
        }

        public async Task<LocationServiceModel> CreeateLocation(LocationServiceModel locationServiceModel)
        {
            Location created = (await this.ideaDbContext.AddAsync(locationServiceModel.ToEntity())).Entity;

            return created.ToServiceModel();
        }

        public async Task GenerateLocationsIfNecessary(Coordinate shipCoordinate)
        {
            if(this.locationEngine.GetLocationsForCoordinate(shipCoordinate).Count == 0)
            {
                Location location = this.locationEngine.GenerateLocation(shipCoordinate);

                await this.ideaDbContext.AddAsync(location);
                await this.ideaDbContext.SaveChangesAsync();
            }
        }

        public async Task<List<LocationServiceModel>> GetAllLocations()
        {
            return await this.ideaDbContext.Locations
                .Include(location => location.LocationType)
                .Include(location => location.Position).ThenInclude(position => position.FrontLowerLeft)
                .Include(location => location.Position).ThenInclude(position => position.FrontLowerRight)
                .Include(location => location.Position).ThenInclude(position => position.FrontUpperLeft)
                .Include(location => location.Position).ThenInclude(position => position.FrontUpperRight)
                .Include(location => location.Position).ThenInclude(position => position.BackLowerLeft)
                .Include(location => location.Position).ThenInclude(position => position.BackLowerRight)
                .Include(location => location.Position).ThenInclude(position => position.BackUpperLeft)
                .Include(location => location.Position).ThenInclude(position => position.BackUpperRight)
                .Select(location => location.ToServiceModel())
                .ToListAsync();
        }

        public async Task<List<LocationServiceModel>> GetLocationsByCoordinate(CoordinateServiceModel coordinateServiceModel)
        {
            return this.locationEngine
                .GetLocationsForCoordinate(coordinateServiceModel.ToEntity())
                .Select(location => location.ToServiceModel())
                .ToList();
        }
    }
}
