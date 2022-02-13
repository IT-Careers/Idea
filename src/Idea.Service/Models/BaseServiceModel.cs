namespace Idea.Service.Models
{
    public abstract class BaseServiceModel
    {
        public BaseServiceModel()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
    }
}
