using Ev.Subsidiaries.Contract.Commands;
using Ev.Subsidiaries.Domain;
using System.Threading;
using System.Threading.Tasks;


namespace Ev.Subsidiaries.Application.Commands
{
    public class DeclineLinkHandler : ICommandHandler<DeclineLinkCommand>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IEventPublisher _eventPublisher;

        public DeclineLinkHandler(ISupplierRepository supplierRepository, IEventPublisher eventPublisher)
        {
            _supplierRepository = supplierRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(DeclineLinkCommand cmd, CancellationToken cancellationToken)
        {
            var supplier = await _supplierRepository.Get(cmd.SubsidiaryId);
            await supplier.DeclineLink(cmd.ParentId, _eventPublisher);
            await _supplierRepository.Save(supplier);
        }
    }
}