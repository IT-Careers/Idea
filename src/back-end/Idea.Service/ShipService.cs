using Idea.Data;
using Idea.Data.Models.Ships;
using Idea.Service.Mapping;
using Idea.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Idea.Service
{
    public class ShipService : IShipService
    {
        private readonly IdeaDbContext ideaDbContext;

        public ShipService(IdeaDbContext ideaDbContext)
        {
            this.ideaDbContext = ideaDbContext;
        }

        public async Task<SpaceshipServiceModel> CreateShipAsync(string userId)
        {
            Spaceship spaceship = new Spaceship
            {
                Capacity = 4,
                FTLCooldown = DateTime.Now,
                FuelPercentage = 100,
                HullIntegrityPercentage = 100,
                X = 0,
                Y = 0,
                Z = 0,
                UserId = userId
            };

            Spaceship created = (await this.ideaDbContext.AddAsync(spaceship))
                .Entity;

            await this.ideaDbContext.SaveChangesAsync();

            return created.ToServiceModel();
        }

        public async Task<SpaceshipServiceModel> GetShipByUserIdAsync(string userId)
        {
            return (await this.ideaDbContext.Ships.Include(ship => ship.User).SingleOrDefaultAsync(ship => ship.UserId == userId))
                .ToServiceModel();
        }

        public async Task<SpaceshipServiceModel> UpdateShipAsync(SpaceshipServiceModel spaceshipServiceModel)
        {
            Spaceship ship = await this.ideaDbContext.Ships
                .Include(ship => ship.User)
                .SingleOrDefaultAsync(ship => ship.Id == spaceshipServiceModel.Id);
        
            ship.Capacity = spaceshipServiceModel.Capacity;
            ship.FuelPercentage = spaceshipServiceModel.FuelPercentage;
            ship.FTLCooldown = spaceshipServiceModel.FTLCooldown;
            ship.HullIntegrityPercentage = spaceshipServiceModel.HullIntegrityPercentage;
            ship.X = spaceshipServiceModel.X;
            ship.Y = spaceshipServiceModel.Y;
            ship.Z = spaceshipServiceModel.Z;

            this.ideaDbContext.Update(ship);
            await this.ideaDbContext.SaveChangesAsync();

            return ship.ToServiceModel();
        }
    }
}
