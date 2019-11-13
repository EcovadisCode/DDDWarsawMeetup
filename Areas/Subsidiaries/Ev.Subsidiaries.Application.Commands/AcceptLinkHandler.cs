using System.Threading;
using System.Threading.Tasks;
using Ev.Subsidiaries.Contract.Commands;
using Ev.Subsidiaries.Domain;

namespace Ev.Subsidiaries.Application.Commands
{
    public class AcceptLinkHandler : ICommandHandler<AcceptLinkCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEventPublisher _eventPublisher;

        public AcceptLinkHandler(ISupplierRepository supplierRepository, IEventPublisher eventPublisher)
        {
            _supplierRepository = supplierRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(AcceptLinkCommand cmd, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.Get(cmd.SubsidiaryId);
            await supplier.AcceptLink(cmd.ParentId, _eventPublisher);
            await _supplierRepository.Save(supplier);
        }
    }
}