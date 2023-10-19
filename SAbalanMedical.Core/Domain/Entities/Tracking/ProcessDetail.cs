
namespace SabalanMedical.Core.Domain.Entities.Tracking
{
    public class ProcessDetail
    {
        public Guid Id { get; set; }
        public Guid ProcessId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductSN { get; set; }
        public int QntyPerPc { get; set; }

    }
}
