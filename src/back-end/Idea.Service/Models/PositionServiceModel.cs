namespace Idea.Service.Models
{
    public class PositionServiceModel : BaseServiceModel
    {
        public string FrontLowerLeftId { get; set; }

        public CoordinateServiceModel FrontLowerLeft { get; set; }

        public string FrontUpperLeftId { get; set; }

        public CoordinateServiceModel FrontUpperLeft { get; set; }

        public string FrontLowerRightId { get; set; }

        public CoordinateServiceModel FrontLowerRight { get; set; }

        public string FrontUpperRightId { get; set; }

        public CoordinateServiceModel FrontUpperRight { get; set; }

        public string BackLowerLeftId { get; set; }

        public CoordinateServiceModel BackLowerLeft { get; set; }

        public string BackUpperLeftId { get; set; }

        public CoordinateServiceModel BackUpperLeft { get; set; }

        public string BackLowerRightId { get; set; }

        public CoordinateServiceModel BackLowerRight { get; set; }

        public string BackUpperRightId { get; set; }

        public CoordinateServiceModel BackUpperRight { get; set; }
    }
}
