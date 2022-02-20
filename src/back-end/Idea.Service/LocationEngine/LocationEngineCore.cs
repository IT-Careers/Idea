using Idea.Data;
using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace Idea.Service.LocationEngine
{
    internal class LocationEngineCore : ILocationEngineCore
    {
        private readonly IdeaDbContext ideaDbContext;

        public LocationEngineCore(IdeaDbContext ideaDbContext)
        {
            this.ideaDbContext = ideaDbContext;
        }

        private bool IsInCubicalStructure(Position position, Coordinate point)
        {
            bool isInFrontLowerX = point.X >= position.FrontLowerLeft.X && point.X <= position.FrontLowerRight.X;
            bool isInFrontUpperX = point.X >= position.FrontUpperLeft.X && point.X <= position.FrontUpperRight.X;
            bool isInBackLowerX = point.X >= position.BackLowerLeft.X && point.X <= position.BackLowerRight.X;
            bool isInBackUpperX = point.X >= position.BackUpperLeft.X && point.X <= position.BackUpperRight.X;

            bool isInFrontX = isInFrontLowerX && isInFrontUpperX;
            bool isInBackX = isInBackLowerX && isInBackUpperX;
            bool isInX = isInFrontX && isInBackX;

            bool isInFrontLeftY = point.Y >= position.FrontLowerLeft.Y && point.Y <= position.FrontUpperLeft.Y;
            bool isInFrontRightY = point.Y >= position.FrontLowerRight.Y && point.Y <= position.FrontUpperRight.Y;
            bool isInBackLeftY = point.Y >= position.BackLowerLeft.Y && point.Y <= position.BackUpperLeft.Y;
            bool isInBackRightY = point.Y >= position.BackLowerRight.Y && point.Y <= position.BackUpperRight.Y;

            bool isInFrontY = isInFrontLeftY && isInFrontRightY;
            bool isInBackY = isInBackLeftY && isInBackRightY;
            bool isInY = isInFrontY && isInBackY;

            bool isInSideLowerLeft = point.Z >= position.FrontLowerLeft.Z && point.Y <= position.BackLowerLeft.Z;
            bool isInSideUpperLeft = point.Y >= position.FrontUpperLeft.Z && point.Y <= position.BackUpperLeft.Z;
            bool isInSideLowerRight = point.Y >= position.FrontLowerRight.Z && point.Y <= position.BackLowerRight.Z;
            bool isInSideUpperRight = point.Y >= position.FrontUpperRight.Z && point.Y <= position.BackUpperRight.Z;

            bool isInLeftZ = isInSideLowerLeft && isInSideUpperLeft;
            bool isInRightZ = isInSideLowerRight && isInSideUpperRight;
            bool isInZ = isInLeftZ && isInRightZ;

            bool isInCube = isInX && isInY && isInZ;

            return isInCube;
        }

        public List<Location> GetLocationsForCoordinate(Coordinate coordinate)
        {
            return this.ideaDbContext.Locations
                .Include(location => location.LocationType)
                .Include(location => location.Position)
                .Where(location => this.IsInCubicalStructure(location.Position, coordinate))
                .ToList();
        }
    }
}
