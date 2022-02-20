namespace Idea.Data.Models.Shared
{
    public class Position : BaseEntity
    {
        public string FrontLowerLeftId { get; set; }

        public virtual Coordinate FrontLowerLeft { get; set; }

        public string FrontUpperLeftId { get; set; }

        public virtual Coordinate FrontUpperLeft { get; set; }

        public string FrontLowerRightId { get; set; }

        public virtual Coordinate FrontLowerRight { get; set; }

        public string FrontUpperRightId { get; set; }

        public virtual Coordinate FrontUpperRight { get; set; }

        public string BackLowerLeftId { get; set; }

        public virtual Coordinate BackLowerLeft { get; set; }

        public string BackUpperLeftId { get; set; }

        public virtual Coordinate BackUpperLeft { get; set; }

        public string BackLowerRightId { get; set; }

        public virtual Coordinate BackLowerRight { get; set; }

        public string BackUpperRightId { get; set; }

        public virtual Coordinate BackUpperRight { get; set; }
    }
}
