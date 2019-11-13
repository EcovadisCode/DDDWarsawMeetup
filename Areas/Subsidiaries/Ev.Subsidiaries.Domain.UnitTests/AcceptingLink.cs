using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Ev.Subsidiaries.Domain.Errors;
using Moq;
using Xunit;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class AcceptingLink
    {
        private static readonly IEventPublisher _publisherStub = new Mock<IEventPublisher>().Object;

        public class GivenLinkWasAlreadyAccepted
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId subsidiaryId, SupplierId parentId)
            {
                var supplier = new Supplier(subsidiaryId);
                await supplier.CreatePendingLink(parentId, _publisherStub);
                await supplier.AcceptLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<AlreadyAccepted>(() => supplier.AcceptLink(parentId, _publisherStub));
            }
        }

        public class GivenLinkWasCreatedAsAccepted
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId subsidiaryId, SupplierId parentId)
            {
                var supplier = new Supplier(subsidiaryId);
                await supplier.CreateAcceptedLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<AlreadyAccepted>(() => supplier.AcceptLink(parentId, _publisherStub));
            }
        }

        public class GivenThereIsNoParent
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId subsidiaryId, SupplierId parentId)
            {
                await Assert.ThrowsAsync<NotLinkedWithGivenParent>(() => new Supplier(subsidiaryId).AcceptLink(parentId, _publisherStub));
            }
        }
    }
}
