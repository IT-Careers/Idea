using Idea.Data.Models.Shared;

namespace Idea.Data.Models.Locations
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }

        public string LocationTypeId { get; set; }
        
        public LocationType LocationType { get; set; }
    
        public string PositionId { get; set; }

        public Position Position { get; set; }
    }
}
