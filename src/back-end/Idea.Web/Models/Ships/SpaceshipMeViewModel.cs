namespace Idea.Web.Models.Ships
{
    public class SpaceshipMeViewModel
    {
        public string Id { get; set; }

        public double Capacity { get; set; }

        public double HullIntegrityPercentage { get; set; }

        public double FuelPercentage { get; set; }

        public DateTime FTLCooldown { get; set; }

        public string Username { get; set; }

        public long X { get; set; }

        public long Y { get; set; }

        public long Z { get; set; }
    }
}
