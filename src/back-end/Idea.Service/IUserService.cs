using Idea.Service.Models;

namespace Idea.Service
{
    public interface IUserService
    {
        Task<IdeaUserServiceModel> CreateAsync(IdeaUserServiceModel ideaUserServiceModel);

        Task<IdeaUserServiceModel> GetByIdAsync(string id);

        Task<IdeaUserServiceModel> AuthenticateAsync(string username, string password);
    }
}
