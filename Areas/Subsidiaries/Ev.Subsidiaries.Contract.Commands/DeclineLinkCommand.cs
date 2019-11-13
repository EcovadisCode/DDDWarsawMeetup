
using Ev.Subsidiaries.Domain;

namespace Ev.Subsidiaries.Contract.Commands
{
    public class DeclineLinkCommand : ICommand
    {
        public DeclineLinkCommand(int subsidiaryId, int parentId)
        {
            SubsidiaryId = new SupplierId(subsidiaryId);
            ParentId = new SupplierId(parentId);
        }

        public SupplierId SubsidiaryId { get; }

        public SupplierId ParentId { get; }
    }
}