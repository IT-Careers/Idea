using Idea.Data.Models.Locations;

namespace Idea.Data.Models.CelestialObjects
{
    public abstract class CelestialObject : BaseEntity
    {
        public string LocationId { get; set; }

        public Location Location { get; set; }
    
        public List<Coordinate> Coordinates { get; set; }
    }
}
