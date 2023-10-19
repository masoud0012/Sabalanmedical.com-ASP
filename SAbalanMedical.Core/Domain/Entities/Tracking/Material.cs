
namespace SabalanMedical.Core.Domain.Entities.Tracking
{
    public class Material
    {
        public Guid Id { get; set; }
        public Guid CatId { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
    }
}
