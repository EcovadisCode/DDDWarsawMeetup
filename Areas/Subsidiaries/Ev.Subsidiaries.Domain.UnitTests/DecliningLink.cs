using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Ev.Subsidiaries.Domain.Errors;
using Moq;
using Xunit;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class DecliningLink
    {
        private static readonly IEventPublisher _publisherStub = new Mock<IEventPublisher>().Object;

        public class GivenLinkIsAlreadyDeclined
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId)
            {
                var supplier = new Supplier(supplierId);
                await supplier.CreatePendingLink(parentId, _publisherStub);
                await supplier.DeclineLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<AlreadyDeclined>(() => supplier.DeclineLink(parentId, _publisherStub));
            }
        }

        public class GivenThereIsNoParent
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId)
            {
                await Assert.ThrowsAsync<NotLinkedWithGivenParent>(() => new Supplier(supplierId).DeclineLink(parentId, _publisherStub));
            }
        }
    }
}