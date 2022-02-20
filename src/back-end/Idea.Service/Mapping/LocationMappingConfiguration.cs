using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;
using Idea.Service.Models;

namespace Idea.Service.Mapping
{
    public static class LocationMappingConfiguration
    {
        public static LocationType ToEntity(this LocationTypeServiceModel locationTypeServiceModel)
        {
            return new LocationType
            {
                Name = locationTypeServiceModel.Name,
            };
        }

        public static LocationTypeServiceModel ToServiceModel(this LocationType locationType)
        {
            return new LocationTypeServiceModel
            {
                Id = locationType.Id,
                Name = locationType.Name
            };
        }

        public static Location ToEntity(this LocationServiceModel locationServiceModel)
        {
            return new Location
            {
                LocationTypeId = locationServiceModel.LocationTypeId,
                Position = locationServiceModel.Position.ToEntity(),
            };
        }

        public static LocationServiceModel ToServiceModel(this Location location)
        {
            return new LocationServiceModel
            {
                Id = location.Id,
                LocationType = location.LocationType.ToServiceModel(),
                Position = location.Position.ToServiceModel()
            };
        }
    }
}
