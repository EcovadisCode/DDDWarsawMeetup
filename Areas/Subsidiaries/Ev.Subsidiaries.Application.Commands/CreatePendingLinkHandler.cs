using System.Linq;
using Ev.Subsidiaries.Contract.Commands;
using Ev.Subsidiaries.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Ev.Subsidiaries.Application.Commands
{
    public class CreatePendingLinkHandler : ICommandHandler<CreatePendingLinkCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEventPublisher _eventPublisher;

        public CreatePendingLinkHandler(ISupplierRepository supplierRepository, IEventPublisher eventPublisher)
        {
            _supplierRepository = supplierRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(CreatePendingLinkCommand cmd, CancellationToken cancellationToken)
        {
            await Task.WhenAll(cmd.SubsidiaryIds.Select(subsidiaryId => CreatePendingLink(subsidiaryId, cmd.ParentId)));
        }

        private async Task CreatePendingLink(SupplierId subsidiaryId, SupplierId parentId)
        {
            var supplier = await _supplierRepository.Get(subsidiaryId);
            await supplier.CreatePendingLink(parentId, _eventPublisher);
            await _supplierRepository.Save(supplier);
        }
    }
}
