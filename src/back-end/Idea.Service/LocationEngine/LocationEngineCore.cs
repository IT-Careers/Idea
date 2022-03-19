using Idea.Data;
using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Idea.Service.LocationEngine
{
    internal class LocationEngineCore : ILocationEngineCore
    {
        private readonly IdeaDbContext ideaDbContext;

        private readonly IRandomService randomService;

        private Dictionary<int, long> locationTypesWithHalfPoints;

        private Dictionary<int, string> locationTypesWithNames;

        public LocationEngineCore(IdeaDbContext ideaDbContext, IRandomService randomService)
        {
            this.ideaDbContext = ideaDbContext;
            this.randomService = randomService;
            this.InitializeLocationTypes();
        }

        private void InitializeLocationTypes()
        {
            this.locationTypesWithHalfPoints = new Dictionary<int, long>();

            locationTypesWithHalfPoints[0] = LocationEngineConstants.StarSystemHalfPoint;
            locationTypesWithHalfPoints[1] = LocationEngineConstants.EmptySpaceHalfPoint;
            locationTypesWithHalfPoints[2] = LocationEngineConstants.NebulaHalfPoint;
            locationTypesWithHalfPoints[3] = LocationEngineConstants.AsteroidFieldHalfPoint;

            this.locationTypesWithNames = new Dictionary<int, string>();

            locationTypesWithNames[0] = "StarSystem";
            locationTypesWithNames[1] = "EmptySpace";
            locationTypesWithNames[2] = "Nebula";
            locationTypesWithNames[3] = "AsteroidBelt";
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

        private long GetActualCoordinateAxisValue(long axisValue, long deltaDiff)
        {
            // Example: 
            // axisValue = -25 000 000
            // deltaDiff = (0 : -15 000 000 000) (for example: 7 500 000 000)
            // -7 475 000 000
            // then calculate axis / math.abs(axis) => always 1 / -1 ... Unless its 0

            return Math.Abs((Math.Abs(axisValue) - Math.Abs(deltaDiff))) * (axisValue == 0 ? 1 : axisValue / Math.Abs(axisValue));
        }
        
        private LocationType GetLocationTypeEntity(int locationType) 
        {
            return this.ideaDbContext.LocationTypes
                .SingleOrDefault(locationTypeFromDb => locationTypeFromDb.Name == this.locationTypesWithNames[locationType]);
        }

        private string GenerateLocationName()
        {
            // TODO: Make it depend on coordinates of location
            long letterLength = this.randomService.RandomNumber(5, 10);
            long digitLength = this.randomService.RandomNumber(1, 2);

            StringBuilder randomLocationName = new StringBuilder();

            for (int i = 0; i < letterLength; i++)
            {
                randomLocationName.Append((char)this.randomService.RandomNumber(65, 90));
            }

            randomLocationName.Append("-");

            for (int i = 0; i < digitLength; i++)
            {
                randomLocationName.Append(this.randomService.RandomNumber(0, 9));
            }

            return randomLocationName.ToString();
        }

        public List<Location> GetLocationsForCoordinate(Coordinate coordinate)
        {
            return this.ideaDbContext.Locations
                .Include(location => location.LocationType)
                .Include(location => location.Position).ThenInclude(position => position.FrontLowerLeft)
                .Include(location => location.Position).ThenInclude(position => position.FrontLowerRight)
                .Include(location => location.Position).ThenInclude(position => position.FrontUpperLeft)
                .Include(location => location.Position).ThenInclude(position => position.FrontUpperRight)
                .Include(location => location.Position).ThenInclude(position => position.BackLowerLeft)
                .Include(location => location.Position).ThenInclude(position => position.BackLowerRight)
                .Include(location => location.Position).ThenInclude(position => position.BackUpperLeft)
                .Include(location => location.Position).ThenInclude(position => position.BackUpperRight)
                .ToList()
                .Where(location => this.IsInCubicalStructure(location.Position, coordinate))
                .ToList();
        }

        // TODO: Refactor this big method
        public Location GenerateLocation(Coordinate coordinate)
        {
            // Generate Location Type
            int locationTypeIndex = (int) this.randomService.RandomNumber(0, 4);
            long locationTypeHalfPoint = this.locationTypesWithHalfPoints[locationTypeIndex];

            // Generate Location Coordinates
            long deltaDiff = this.randomService.RandomNumber(0, locationTypeHalfPoint);

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = GetActualCoordinateAxisValue(coordinate.X, deltaDiff),
                Y = GetActualCoordinateAxisValue(coordinate.Y, deltaDiff),
                Z = GetActualCoordinateAxisValue(coordinate.Z, deltaDiff)
            };

            Coordinate frontLowerRightPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X + (2 * locationTypeHalfPoint),
                Y = frontLowerLeftPoint.Y,
                Z = frontLowerLeftPoint.Z
            };

            Coordinate frontUpperLeftPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X,
                Y = frontLowerLeftPoint.Y + (2 * locationTypeHalfPoint),
                Z = frontLowerLeftPoint.Z
            };

            Coordinate frontUpperRightPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X + (2 * locationTypeHalfPoint),
                Y = frontLowerLeftPoint.Y + (2 * locationTypeHalfPoint),
                Z = frontLowerLeftPoint.Z
            };

            Coordinate backLowerLeftPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X,
                Y = frontLowerLeftPoint.Y,
                Z = frontLowerLeftPoint.Z + (2 * locationTypeHalfPoint)
            };

            Coordinate backLowerRightPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X + (2 * locationTypeHalfPoint),
                Y = frontLowerLeftPoint.Y,
                Z = frontLowerLeftPoint.Z + (2 * locationTypeHalfPoint)
            };

            Coordinate backUpperLeftPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X,
                Y = frontLowerLeftPoint.Y + (2 * locationTypeHalfPoint),
                Z = frontLowerLeftPoint.Z + (2 * locationTypeHalfPoint)
            };

            Coordinate backUpperRightPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X + (2 * locationTypeHalfPoint),
                Y = frontLowerLeftPoint.Y + (2 * locationTypeHalfPoint),
                Z = frontLowerLeftPoint.Z + (2 * locationTypeHalfPoint)
            };

            Position locationPosition = new Position
            {
                FrontLowerLeft = frontLowerLeftPoint,
                FrontLowerRight = frontLowerRightPoint,
                FrontUpperLeft = frontUpperLeftPoint,
                FrontUpperRight = frontUpperRightPoint,
                BackLowerLeft = backLowerLeftPoint,
                BackLowerRight = backLowerRightPoint,
                BackUpperLeft = backUpperLeftPoint,
                BackUpperRight = backUpperRightPoint
            };

            LocationType locationType = this.GetLocationTypeEntity(locationTypeIndex);

            string locationName = this.GenerateLocationName();

            Location location = new Location
            {
                Name = locationName,
                LocationType = locationType,
                Position = locationPosition
            };

            return location;
        }
    }
}
