namespace Idea.Data.Models.Materials
{
    public class ComplexMaterial : BaseEntity
    {
        public string Name { get; set; }

        public List<ComplexMaterialRequirement> Requirements { get; set; }
    }
}
