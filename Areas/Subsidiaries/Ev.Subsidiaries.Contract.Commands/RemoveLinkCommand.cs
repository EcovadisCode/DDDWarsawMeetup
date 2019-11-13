using System.Collections.Generic;
using System.Linq;
using Ev.Subsidiaries.Domain;

namespace Ev.Subsidiaries.Contract.Commands
{
    public class RemoveLinkCommand : ICommand
    {
        public RemoveLinkCommand(IEnumerable<int> subsidiaryIds, int parentId)
        {
            SubsidiaryIds = subsidiaryIds?.Select(id => new SupplierId(id));
            ParentId = new SupplierId(parentId);
        }

        public IEnumerable<SupplierId> SubsidiaryIds { get; }

        public SupplierId ParentId { get; }
    }
}