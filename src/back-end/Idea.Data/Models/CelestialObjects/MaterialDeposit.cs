using Idea.Data.Models.Materials;

namespace Idea.Data.Models.CelestialObjects
{
    public class MaterialDeposit : BaseEntity
    {
        public string PlanetaryId { get; set; }

        public Planetary Planetary { get; set; }

        public int Quantity { get; set; }

        public string BasicMaterialId { get; set;}
        
        public BasicMaterial BasicMaterial { get; set; }
    }
}
