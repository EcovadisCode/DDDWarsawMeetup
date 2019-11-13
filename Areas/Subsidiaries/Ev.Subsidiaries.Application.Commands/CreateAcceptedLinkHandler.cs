using Ev.Subsidiaries.Contract.Commands;
using Ev.Subsidiaries.Domain;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ev.Subsidiaries.Application.Commands
{
    public class CreateAcceptedLinkHandler : ICommandHandler<CreateAcceptedLinkCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEventPublisher _eventPublisher;

        public CreateAcceptedLinkHandler(ISupplierRepository supplierRepository, IEventPublisher eventPublisher)
        {
            _supplierRepository = supplierRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(CreateAcceptedLinkCommand cmd, CancellationToken cancellationToken)
        {
            await Task.WhenAll(cmd.SubsidiaryIds.Select(subsidiaryId => CreateAcceptedLink(subsidiaryId, cmd.ParentId)));
        }

        private async Task CreateAcceptedLink(SupplierId subsidiaryId, SupplierId parentId)
        {
            var supplier = await _supplierRepository.Get(subsidiaryId);
            await supplier.CreateAcceptedLink(parentId, _eventPublisher);
            await _supplierRepository.Save(supplier);
        }
    }
}