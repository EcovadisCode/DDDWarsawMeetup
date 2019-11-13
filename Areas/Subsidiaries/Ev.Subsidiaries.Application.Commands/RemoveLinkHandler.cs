using System.Linq;
using Ev.Subsidiaries.Contract.Commands;
using Ev.Subsidiaries.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Ev.Subsidiaries.Application.Commands
{
    public class RemoveLinkHandler : ICommandHandler<RemoveLinkCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEventPublisher _eventPublisher;

        public RemoveLinkHandler(ISupplierRepository supplierRepository, IEventPublisher eventPublisher)
        {
            _supplierRepository = supplierRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(RemoveLinkCommand cmd, CancellationToken cancellationToken)
        {
            await Task.WhenAll(cmd.SubsidiaryIds.Select(subsidiaryId => RemoveLink(subsidiaryId, cmd.ParentId)));
        }

        private async Task RemoveLink(SupplierId subsidiaryId, SupplierId parentId)
        {
            var supplier = await _supplierRepository.Get(subsidiaryId);
            await supplier.RemoveLink(parentId, _eventPublisher);
            await _supplierRepository.Save(supplier);
        }
    }
}