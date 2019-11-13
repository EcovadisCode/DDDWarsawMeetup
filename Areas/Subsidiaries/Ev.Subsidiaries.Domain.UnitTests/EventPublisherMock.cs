using System;
using System.Threading.Tasks;
using Ev.Subsidiaries.Domain.Events;
using Moq;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class EventPublisherMock<TEvent> where TEvent : IDomainEvent
    {
        private readonly Mock<IEventPublisher> _mock;
        private TEvent _publishedEvent;

        public IEventPublisher Object => _mock.Object;

        public EventPublisherMock()
        {
            _mock = new Mock<IEventPublisher>();
            _mock.Setup(pub => pub.Publish(It.IsAny<TEvent>()))
                .Callback((IDomainEvent @event) =>
                {
                    _publishedEvent = (TEvent)@event;
                })
                .Returns(Task.CompletedTask);
        }

        public void VerifyPublishedEvent(Action<TEvent> assertEvent)
        {
            _mock.Verify(pub => pub.Publish(It.IsAny<TEvent>()), Times.Once());
            assertEvent(_publishedEvent);
        }
    }
}
