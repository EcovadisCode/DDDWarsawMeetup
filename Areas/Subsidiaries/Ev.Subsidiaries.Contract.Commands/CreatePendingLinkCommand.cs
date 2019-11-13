using System.Collections.Generic;
using System.Linq;
using Ev.Subsidiaries.Domain;

namespace Ev.Subsidiaries.Contract.Commands
{
    public class CreatePendingLinkCommand : ICommand
    {
        public CreatePendingLinkCommand(int parentId, IEnumerable<int> subsidiaryIds)
        {
            ParentId = new SupplierId(parentId);
            SubsidiaryIds = subsidiaryIds?.Select(id => new SupplierId(id));
        }

        public SupplierId ParentId { get; }

        public IEnumerable<SupplierId> SubsidiaryIds { get; }
    }
}