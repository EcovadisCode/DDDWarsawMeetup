using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Ev.Subsidiaries.Domain.Errors;
using Moq;
using Xunit;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class CreatingLink
    {
        private static readonly IEventPublisher _publisherStub = new Mock<IEventPublisher>().Object;

        public class GivenPendingLinkIsAlreadyCreated
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId, SupplierId secondParentId)
            {
                var supplier = new Supplier(supplierId);
                await supplier.CreatePendingLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<AlreadyLinkedWithParent>(() => supplier.CreatePendingLink(secondParentId, _publisherStub));
            }
        }

        public class GivenAcceptedLinkIsAlreadyCreated
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId, SupplierId secondParentId)
            {
                var supplier = new Supplier(supplierId);
                await supplier.CreateAcceptedLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<AlreadyLinkedWithParent>(() => supplier.CreateAcceptedLink(secondParentId, _publisherStub));
            }
        }

        public class GivenSupplierHasSubsidiaries
        {
            [Theory]
            [AutoData]
            public async Task ThenAcceptedLinkCreationWithChildIsDenied(SupplierId supplierId, SupplierId childId)
            {
                var supplier = new Supplier(supplierId, new List<SupplierId> {childId});

                await Assert.ThrowsAsync<CannotLinkWithChild>(() => supplier.CreateAcceptedLink(childId, _publisherStub));
            }

            [Theory]
            [AutoData]
            public async Task ThenPendingLinkCreationWithChildIsDenied(SupplierId supplierId, SupplierId childId)
            {
                var supplier = new Supplier(supplierId, new List<SupplierId> {childId});

                await Assert.ThrowsAsync<CannotLinkWithChild>(() => supplier.CreatePendingLink(childId, _publisherStub));
            }
        }
    }
}
