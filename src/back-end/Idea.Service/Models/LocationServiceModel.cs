namespace Idea.Service.Models
{
    public class LocationServiceModel : BaseServiceModel
    {
        public string Name { get; set; }

        public string LocationTypeId { get; set; }

        public LocationTypeServiceModel LocationType { get; set; }

        public string PositionId { get; set; }

        public PositionServiceModel Position { get; set; }

        public string TravelCoordinateId { get; set; }

        public CoordinateServiceModel TravelCoordinate { get; set; }
    }
}
