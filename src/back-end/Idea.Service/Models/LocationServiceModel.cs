namespace Idea.Service.Models
{
    public class LocationServiceModel : BaseServiceModel
    {
        public string LocationTypeId { get; set; }

        public LocationTypeServiceModel LocationType { get; set; }

        public List<CoordinateServiceModel> Coordinates { get; set; }
    }
}
