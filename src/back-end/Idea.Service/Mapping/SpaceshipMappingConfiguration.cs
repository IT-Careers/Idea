using Idea.Data.Models.Ships;
using Idea.Service.Models;

namespace Idea.Service.Mapping
{
    public static class SpaceshipMappingConfiguration
    {
        public static Spaceship ToEntity(this SpaceshipServiceModel spaceshipServiceModel)
        {
            return new Spaceship
            {
                Id = spaceshipServiceModel.Id,
                Capacity = spaceshipServiceModel.Capacity,
                FTLCooldown = spaceshipServiceModel.FTLCooldown,
                FuelPercentage = spaceshipServiceModel.FuelPercentage,  
                HullIntegrityPercentage = spaceshipServiceModel.HullIntegrityPercentage,
                UserId = spaceshipServiceModel.UserId,
                X = spaceshipServiceModel.X,
                Y = spaceshipServiceModel.Y,
                Z = spaceshipServiceModel.Z
            };
        }

        public static SpaceshipServiceModel ToServiceModel(this Spaceship spaceship)
        {
            return new SpaceshipServiceModel
            {
                Id = spaceship.Id,
                Capacity = spaceship.Capacity,
                FTLCooldown = spaceship.FTLCooldown,
                FuelPercentage = spaceship.FuelPercentage,
                HullIntegrityPercentage = spaceship.HullIntegrityPercentage,
                UserId = spaceship.UserId,
                User = spaceship.User.ToServiceModel(),
                X = spaceship.X,
                Y = spaceship.Y,
                Z = spaceship.Z
            };
        }
    }
}
