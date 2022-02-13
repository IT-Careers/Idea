using Idea.Data.Models;
using Idea.Service.Models;

namespace Idea.Service.Mapping
{
    public static class IdeaUserMappingConfiguration
    {
        public static IdeaUser ToEntity(this IdeaUserServiceModel ideaUserServiceModel)
        {
            return new IdeaUser
            {
                Username = ideaUserServiceModel.Username,
                Password = ideaUserServiceModel.Password
            };
        }

        public static IdeaUserServiceModel ToServiceModel(this IdeaUser ideaUser)
        {
            return new IdeaUserServiceModel
            {
                Id = ideaUser.Id,
                Username = ideaUser.Username,
                Password = ideaUser.Password
            };
        }
    }
}
