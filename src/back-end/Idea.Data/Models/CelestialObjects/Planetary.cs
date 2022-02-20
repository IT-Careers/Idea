namespace Idea.Data.Models.CelestialObjects
{
    public class Planetary : CelestialObject
    {
        public string Atmosphere { get; set; }

        public bool IsHabitable { get; set; }

        public string PlanetaryTypeId { get; set; } 

        public PlanetaryType PlanetaryType { get; set; }
    }
}
