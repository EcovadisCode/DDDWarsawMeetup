namespace Ev.Subsidiaries.Domain.Events
{
    public class LinkDeclined : IDomainEvent
    {
        public SupplierId SubsidiaryId { get; }
        public SupplierId ParentId { get; }


        public LinkDeclined(SupplierId subsidiaryId, SupplierId parentId)
        {
            SubsidiaryId = subsidiaryId;
            ParentId = parentId;
        }
    }
}