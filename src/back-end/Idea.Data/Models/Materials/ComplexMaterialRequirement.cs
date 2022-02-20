namespace Idea.Data.Models.Materials
{
    public class ComplexMaterialRequirement : BaseEntity
    {
        public string ComplexMaterialId { get; set; }

        public ComplexMaterial ComplexMaterial { get; set; }
   
        public string BasicMaterialId { get; set; }

        public BasicMaterial BasicMaterial { get; set; }
    }
}
