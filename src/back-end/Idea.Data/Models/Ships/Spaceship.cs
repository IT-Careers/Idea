namespace Idea.Data.Models.Ships
{
    public class Spaceship : BaseEntity
    {
        public double Capacity { get; set; }

        public double HullIntegrityPercentage { get; set; }

        public double FuelPercentage { get; set; }

        public DateTime FTLCooldown { get; set; }

        public string UserId { get; set; }

        public IdeaUser User { get; set; }

        public long X { get; set; }

        public long Y { get; set; }

        public long Z { get; set; }
    }
}
