using Idea.Service.Models;

namespace Idea.Service
{
    public interface IShipService
    {
        Task<SpaceshipServiceModel> CreateShipAsync(string userId);

        Task<SpaceshipServiceModel> GetShipByUserIdAsync(string userId);

        Task<SpaceshipServiceModel> UpdateShipAsync(SpaceshipServiceModel spaceshipServiceModel);
    }
}
