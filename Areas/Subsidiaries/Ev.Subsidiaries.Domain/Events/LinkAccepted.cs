namespace Ev.Subsidiaries.Domain.Events
{
    public class LinkAccepted : IDomainEvent
    {
        public SupplierId SubsidiaryId { get; }
        public SupplierId ParentId { get; }

        public LinkAccepted(SupplierId supplierId, SupplierId parentId)
        {
            SubsidiaryId = supplierId;
            ParentId = parentId;
        }
    }
}