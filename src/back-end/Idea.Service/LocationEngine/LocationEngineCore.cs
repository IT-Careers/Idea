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

        private Position GenerateFrontLowerLeftLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = coordinate.X - deltaDiff,
                Y = coordinate.Y - deltaDiff,
                Z = coordinate.Z - deltaDiff
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

            return locationPosition;
        }

        private Position GenerateBackLowerLeftLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate backLowerLeftPoint = new Coordinate
            {
                X = coordinate.X - deltaDiff,
                Y = coordinate.Y - deltaDiff,
                Z = coordinate.Z + deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = backLowerLeftPoint.X,
                Y = backLowerLeftPoint.Y,
                Z = backLowerLeftPoint.Z - (2 * locationTypeHalfPoint)
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

            return locationPosition;
        }

        private Position GenerateFrontLowerRightLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate frontLowerRightPoint = new Coordinate
            {
                X = coordinate.X + deltaDiff,
                Y = coordinate.Y - deltaDiff,
                Z = coordinate.Z - deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = frontLowerRightPoint.X - (2 * locationTypeHalfPoint),
                Y = frontLowerRightPoint.Y,
                Z = frontLowerRightPoint.Z
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

            return locationPosition;
        }

        private Position GenerateBackLowerRightLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate backLowerRightPoint = new Coordinate
            {
                X = coordinate.X + deltaDiff,
                Y = coordinate.Y - deltaDiff,
                Z = coordinate.Z + deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = backLowerRightPoint.X - (2 * locationTypeHalfPoint),
                Y = backLowerRightPoint.Y,
                Z = backLowerRightPoint.Z - (2 * locationTypeHalfPoint)
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

            return locationPosition;
        }

        private Position GenerateFrontUpperLeftLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate frontUpperLeftPoint = new Coordinate
            {
                X = coordinate.X - deltaDiff,
                Y = coordinate.Y + deltaDiff,
                Z = coordinate.Z - deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = frontUpperLeftPoint.X,
                Y = frontUpperLeftPoint.Y - (2 * locationTypeHalfPoint),
                Z = frontUpperLeftPoint.Z
            };

            Coordinate frontLowerRightPoint = new Coordinate
            {
                X = frontLowerLeftPoint.X + (2 * locationTypeHalfPoint),
                Y = frontLowerLeftPoint.Y,
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

            return locationPosition;
        }

        private Position GenerateFrontUpperRightLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate frontUpperRightPoint = new Coordinate
            {
                X = coordinate.X + deltaDiff,
                Y = coordinate.Y + deltaDiff,
                Z = coordinate.Z - deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = frontUpperRightPoint.X - (2 * locationTypeHalfPoint),
                Y = frontUpperRightPoint.Y - (2 * locationTypeHalfPoint),
                Z = frontUpperRightPoint.Z
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

            return locationPosition;
        }

        private Position GenerateBackUpperLeftLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate backUpperLeftPoint = new Coordinate
            {
                X = coordinate.X - deltaDiff,
                Y = coordinate.Y + deltaDiff,
                Z = coordinate.Z + deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = backUpperLeftPoint.X,
                Y = backUpperLeftPoint.Y - (2 * locationTypeHalfPoint),
                Z = backUpperLeftPoint.Z - (2 * locationTypeHalfPoint)
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

            return locationPosition;
        }

        private Position GenerateBackUpperRightLeading(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint)
        {
            Coordinate backUpperRightPoint = new Coordinate
            {
                X = coordinate.X + deltaDiff,
                Y = coordinate.Y + deltaDiff,
                Z = coordinate.Z + deltaDiff
            };

            Coordinate frontLowerLeftPoint = new Coordinate
            {
                X = backUpperRightPoint.X - (2 * locationTypeHalfPoint),
                Y = backUpperRightPoint.Y - (2 * locationTypeHalfPoint),
                Z = backUpperRightPoint.Z - (2 * locationTypeHalfPoint)
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

            return locationPosition;
        }

        private Position GenerateLocationPositionRelativeToOctant(Coordinate coordinate, long deltaDiff, long locationTypeHalfPoint, Location location)
        {
            bool isFrontLowerLeftLeading = coordinate.X >= 0 && coordinate.Y >= 0 && coordinate.Z >= 0;
            bool isBackLowerLeftLeading = coordinate.X >= 0 && coordinate.Y >= 0 && coordinate.Z < 0;
            bool isFrontLowerRightLeading = coordinate.X < 0 && coordinate.Y >= 0 && coordinate.Z >= 0;
            bool isBackLowerRightLeading = coordinate.X < 0 && coordinate.Y >= 0 && coordinate.Z < 0;

            bool isFrontUpperLeftLeading = coordinate.X >= 0 && coordinate.Y < 0 && coordinate.Z >= 0;
            bool isBackUpperLeftLeading = coordinate.X >= 0 && coordinate.Y < 0 && coordinate.Z < 0;
            bool isFrontUpperRightLeading = coordinate.X < 0 && coordinate.Y < 0 && coordinate.Z >= 0;
            bool isBackUpperRightLeading = coordinate.X < 0 && coordinate.Y < 0 && coordinate.Z < 0;

            Position locationPosition = null;

            if (isFrontLowerLeftLeading)
            {
                locationPosition = this.GenerateFrontLowerLeftLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.FrontLowerLeft;
            }
            else if (isFrontLowerRightLeading)
            {
                locationPosition = this.GenerateFrontLowerRightLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.FrontLowerRight;
            }
            else if (isFrontUpperLeftLeading)
            {
                locationPosition = this.GenerateFrontUpperLeftLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.FrontUpperLeft;
            }
            else if (isFrontUpperRightLeading)
            {
                locationPosition = this.GenerateFrontUpperRightLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.FrontUpperRight;
            }
            else if (isBackLowerLeftLeading)
            {
                locationPosition = this.GenerateBackLowerLeftLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.BackLowerLeft;
            }
            else if (isBackLowerRightLeading)
            {
                locationPosition = this.GenerateBackLowerRightLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.BackLowerRight;
            }
            else if (isBackUpperLeftLeading)
            {
                locationPosition = this.GenerateBackUpperLeftLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.BackUpperLeft;
            }
            else if(isBackUpperRightLeading)
            {
                locationPosition = this.GenerateBackUpperRightLeading(coordinate, deltaDiff, locationTypeHalfPoint);
                location.TravelCoordinate = locationPosition.BackUpperRight;
            }

            location.Position = locationPosition;

            return locationPosition;
        }

        public Location GenerateLocation(Coordinate coordinate)
        {
            // Generate Location Type
            int locationTypeIndex = 0;
            long locationTypeHalfPoint = this.locationTypesWithHalfPoints[locationTypeIndex];

            LocationType locationType = this.GetLocationTypeEntity(locationTypeIndex);

            string locationName = this.GenerateLocationName();

            Location location = new Location
            {
                Name = locationName,
                LocationType = locationType,
            };

            // Generate Location Coordinates
            long deltaDiff = this.randomService.RandomNumber(0, locationTypeHalfPoint);

            this.GenerateLocationPositionRelativeToOctant(coordinate, deltaDiff, locationTypeHalfPoint, location);

            return location;
        }
    }
}