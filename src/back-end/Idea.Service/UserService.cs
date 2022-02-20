using Idea.Data;
using Idea.Data.Models;
using Idea.Service.Mapping;
using Idea.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Idea.Service
{
    public class UserService : IUserService
    {
        private readonly IdeaDbContext ideaDbContext;

        private readonly IShipService shipService;

        public UserService(IdeaDbContext ideaDbContext, IShipService shipService)
        {
            this.ideaDbContext = ideaDbContext;
            this.shipService = shipService;
        }

        public async Task<IdeaUserServiceModel> AuthenticateAsync(string username, string password)
        {
            IdeaUser user = await this.ideaDbContext.Users.SingleOrDefaultAsync(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }

            // authentication successful
            return user.ToServiceModel();
        }

        public async Task<IdeaUserServiceModel> CreateAsync(IdeaUserServiceModel ideaUserServiceModel)
        {

            ideaUserServiceModel.Password = BCrypt.Net.BCrypt.HashPassword(ideaUserServiceModel.Password);

            IdeaUserServiceModel created = (await this.ideaDbContext.AddAsync(ideaUserServiceModel.ToEntity())).Entity.ToServiceModel();
            
            await this.ideaDbContext.SaveChangesAsync();

            await shipService.CreateShipAsync(created.Id);

            return created;
        }

        public async Task<IdeaUserServiceModel> GetByIdAsync(string id)
        {
            return (await this.ideaDbContext.Users.SingleOrDefaultAsync(user => user.Id == id)).ToServiceModel();
        }
    }
}
