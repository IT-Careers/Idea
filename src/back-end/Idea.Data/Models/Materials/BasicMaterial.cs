namespace Idea.Data.Models.Materials
{
    public class BasicMaterial : BaseEntity
    {
        public string Name { get; set; }

        public bool IsProcessed { get; set; }
    }
}
