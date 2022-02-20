using Idea.Data.Models.Shared;
using Idea.Service.Models;

namespace Idea.Service.Mapping
{
    public static class PositionMappingConfiguration
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

        public static CoordinateServiceModel ToServiceModel(this Coordinate coordinate)
        {
            return new CoordinateServiceModel { 
                Id = coordinate.Id,
                X = coordinate.X,
                Y = coordinate.Y,
                Z = coordinate.Z 
            };
        }

        public static Position ToEntity(this PositionServiceModel positionServiceModel)
        {
            return new Position
            {
                FrontLowerLeftId = positionServiceModel.FrontLowerLeftId,
                FrontLowerRightId = positionServiceModel.FrontLowerRightId,
                FrontUpperLeftId = positionServiceModel.FrontUpperLeftId,
                FrontUpperRightId = positionServiceModel.FrontUpperRightId,
                BackLowerLeftId = positionServiceModel.BackLowerLeftId,
                BackLowerRightId = positionServiceModel.BackLowerRightId,
                BackUpperLeftId = positionServiceModel.BackUpperLeftId,
                BackUpperRightId = positionServiceModel.BackUpperRightId    
            };
        }

        public static PositionServiceModel ToServiceModel(this Position position)
        {
            return new PositionServiceModel
            {
                FrontLowerLeftId = position.FrontLowerLeftId,
                FrontLowerLeft = position.FrontLowerLeft.ToServiceModel(),

                FrontLowerRightId = position.FrontLowerRightId,
                FrontLowerRight = position.FrontLowerRight.ToServiceModel(),

                FrontUpperLeftId = position.FrontUpperLeftId,
                FrontUpperLeft = position.FrontUpperLeft.ToServiceModel(),

                FrontUpperRightId = position.FrontUpperRightId,
                FrontUpperRight = position.FrontUpperRight.ToServiceModel(),

                BackLowerLeftId = position.BackLowerLeftId,
                BackLowerLeft = position.BackLowerLeft.ToServiceModel(),

                BackLowerRightId = position.BackLowerRightId,
                BackLowerRight = position.BackLowerRight.ToServiceModel(),

                BackUpperLeftId = position.BackUpperLeftId,
                BackUpperLeft = position.BackUpperLeft.ToServiceModel(),

                BackUpperRightId = position.BackUpperRightId,
                BackUpperRight = position.BackUpperRight.ToServiceModel()
            };
        }
    }
}
