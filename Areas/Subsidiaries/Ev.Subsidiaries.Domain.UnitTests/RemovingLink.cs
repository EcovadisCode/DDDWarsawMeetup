using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Ev.Subsidiaries.Domain.Errors;
using Moq;
using Xunit;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class RemovingLink
    {
        private static readonly IEventPublisher _publisherStub = new Mock<IEventPublisher>().Object;

        public class GivenThereIsNoParent
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId)
            {
                await Assert.ThrowsAsync<NotLinkedWithGivenParent>(() => new Supplier(supplierId).RemoveLink(parentId, _publisherStub));
            }
        }

        public class GivenLinkWasAlreadyRemoved
        {
            [Theory]
            [AutoData]
            public async Task ThenActionIsDenied(SupplierId supplierId, SupplierId parentId)
            {
                var supplier = new Supplier(supplierId);
                await supplier.CreateAcceptedLink(parentId, _publisherStub);
                await supplier.RemoveLink(parentId, _publisherStub);

                await Assert.ThrowsAsync<NotLinkedWithGivenParent>(() => supplier.RemoveLink(parentId, _publisherStub));
            }
        }
    }
}