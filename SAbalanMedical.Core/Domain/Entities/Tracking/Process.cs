

namespace SabalanMedical.Core.Domain.Entities.Tracking
{
    public class Process
    {
       public Guid Id { get; set; }
        public Guid ProcessNameId { get; set; }
        public Guid MachineId { get; set; }
        public Guid PersonId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set;}
        public string DateTime { get;set; }
        public string SN { get; set; }
        public string Description { get; set; }
    }
}
