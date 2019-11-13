using System.Threading.Tasks;
using Ev.Subsidiaries.Domain.Events;

namespace Ev.Subsidiaries.Domain
{
    public interface IEventPublisher
    {
        Task Publish(IDomainEvent domainEvent);
    }
}