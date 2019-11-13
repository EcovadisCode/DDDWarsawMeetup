using Ev.Subsidiaries.Domain;

namespace Ev.Subsidiaries.Contract.Commands
{
    public class AcceptLinkCommand : ICommand
    {
        public AcceptLinkCommand(int subsidiaryId, int parentId)
        {
            SubsidiaryId = new SupplierId(subsidiaryId);
            ParentId = new SupplierId(parentId);
        }

        public SupplierId SubsidiaryId { get; }

        public SupplierId ParentId { get; }
    }
}