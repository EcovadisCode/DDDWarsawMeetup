namespace Ev.Subsidiaries.Domain.Events
{
    public class LinkRemoved : IDomainEvent
    {
        public SupplierId SubsidiaryId { get; }
        public SupplierId ParentId { get; }

        public LinkRemoved(SupplierId supplierId, SupplierId parentId)
        {
            SubsidiaryId = supplierId;
            ParentId = parentId;
        }
    }
}
