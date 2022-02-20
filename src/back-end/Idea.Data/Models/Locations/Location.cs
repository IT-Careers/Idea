namespace Idea.Data.Models.Locations
{
    public class Location : BaseEntity
    {
        public string LocationTypeId { get; set; }
        
        public LocationType LocationType { get; set; }
    
        public List<Coordinate> Coordinates { get; set; } // Always 8
    }
}
