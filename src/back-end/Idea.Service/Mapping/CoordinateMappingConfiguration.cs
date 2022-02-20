using Idea.Data.Models.Locations;
using Idea.Service.Models;

namespace Idea.Service.Mapping
{
    public static class CoordinateMappingConfiguration
    {
        public static Coordinate ToEntity(this CoordinateServiceModel coordinateServiceModel)
        {
            return new Coordinate
            {
                X = coordinateServiceModel.X,
                Y = coordinateServiceModel.Y,
                Z = coordinateServiceModel.Z
            };
        }

        public static CoordinateServiceModel ToEntity(this Coordinate coordinate)
        {
            return new CoordinateServiceModel { 
                Id = coordinate.Id,
                X = coordinate.X,
                Y = coordinate.Y,
                Z = coordinate.Z 
            };
        }
    }
}
