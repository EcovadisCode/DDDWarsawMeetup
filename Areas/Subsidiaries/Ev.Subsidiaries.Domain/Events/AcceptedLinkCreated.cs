namespace Ev.Subsidiaries.Domain.Events
{
    public class AcceptedLinkCreated : IDomainEvent
    {
        public SupplierId SubsidiaryId { get; }
        public SupplierId ParentId { get; }

        public AcceptedLinkCreated(SupplierId subsidiaryId, SupplierId parentId)
        {
            SubsidiaryId = subsidiaryId;
            ParentId = parentId;
        }
    }
}