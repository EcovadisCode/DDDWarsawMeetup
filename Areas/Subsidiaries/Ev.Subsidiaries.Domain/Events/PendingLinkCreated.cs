namespace Ev.Subsidiaries.Domain.Events
{
    public class PendingLinkCreated : IDomainEvent
    {
        public SupplierId SubsidiaryId { get; }
        public SupplierId ParentId { get; }

        public PendingLinkCreated(SupplierId subsidiaryId, SupplierId parentId)
        {
            SubsidiaryId = subsidiaryId;
            ParentId = parentId;
        }
    }
}