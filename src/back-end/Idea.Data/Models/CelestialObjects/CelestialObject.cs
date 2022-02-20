using Idea.Data.Models.Locations;
using Idea.Data.Models.Shared;

namespace Idea.Data.Models.CelestialObjects
{
    public abstract class CelestialObject : BaseEntity
    {
        public string LocationId { get; set; }

        public Location Location { get; set; }

        public string PositionId { get; set; }

        public Position Position { get; set; }
    }
}
