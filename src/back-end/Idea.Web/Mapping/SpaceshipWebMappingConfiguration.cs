using Idea.Service.Models;
using Idea.Web.Models.Ships;

namespace Idea.Web.Mapping
{
    public static class SpaceshipWebMappingConfiguration
    {
        public static SpaceshipMeViewModel ToSpaceshipMeViewModel(this SpaceshipServiceModel spaceshipServiceModel)
        {
            return new SpaceshipMeViewModel
            {
                Id = spaceshipServiceModel.Id,
                X = spaceshipServiceModel.X,
                Y = spaceshipServiceModel.Y,
                Z = spaceshipServiceModel.Z,
                Capacity = spaceshipServiceModel.Capacity,
                HullIntegrityPercentage = spaceshipServiceModel.HullIntegrityPercentage,
                FuelPercentage = spaceshipServiceModel.FuelPercentage,
                FTLCooldown = spaceshipServiceModel.FTLCooldown,
                Username = spaceshipServiceModel.User.Username                
            };
        }
    }
}
