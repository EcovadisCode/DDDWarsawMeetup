using Ev.Subsidiaries.Domain.Errors;
using Ev.Subsidiaries.Domain.Events;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ev.Subsidiaries.Domain
{
    public class Supplier : AggregateRoot<SupplierId>
    {
        private Option<SupplierId> _parentId;
        private Option<RelationshipStatus> _status;
        private readonly IReadOnlyList<SupplierId> _children;

        internal Supplier(SupplierId supplierId) : this(supplierId, new List<SupplierId>())
            => AssertStateIsConsistent();

        internal Supplier(SupplierId supplierId, IReadOnlyList<SupplierId> children) : base(supplierId)
        {
            _children = children;
            AssertStateIsConsistent();
        }

        internal Supplier(SupplierId supplierId, Option<SupplierId> parentId, Option<RelationshipStatus> relationshipStatus, IReadOnlyList<SupplierId> children)
            : this(supplierId, children)
        {
            _parentId = parentId;
            _status = relationshipStatus;
            AssertStateIsConsistent();
        }

        public async Task CreatePendingLink(SupplierId parentId, IEventPublisher eventPublisher)
        {
            if (IsLinkedWithParent)
                throw new AlreadyLinkedWithParent();
            if (_children.Contains(parentId))
                throw new CannotLinkWithChild();

            _parentId = parentId;
            _status = RelationshipStatus.Pending;
            await eventPublisher.Publish(new PendingLinkCreated(Identifier, _parentId.ValueUnsafe()));
        }

        public async Task CreateAcceptedLink(SupplierId parentId, IEventPublisher eventPublisher)
        {
            if (IsLinkedWithParent)
                throw new AlreadyLinkedWithParent();
            if (_children.Contains(parentId))
                throw new CannotLinkWithChild();

            _parentId = parentId;
            _status = RelationshipStatus.Accepted;
            await eventPublisher.Publish(new AcceptedLinkCreated(Identifier, _parentId.ValueUnsafe()));
        }

        public async Task DeclineLink(SupplierId parentId, IEventPublisher eventPublisher)
        {
            if (_parentId != parentId)
                throw new NotLinkedWithGivenParent();
            if (!StatusAllowsToDeclineLink)
                throw new AlreadyDeclined();

            _status = RelationshipStatus.Declined;
            await eventPublisher.Publish(new LinkDeclined(Identifier, _parentId.ValueUnsafe()));
        }

        public async Task AcceptLink(SupplierId parentId, IEventPublisher eventPublisher)
        {
            if (_parentId != parentId)
                throw new NotLinkedWithGivenParent();
            if (!StatusAllowsToAcceptLink)
                throw new AlreadyAccepted();

            _status = RelationshipStatus.Accepted;
            await eventPublisher.Publish(new LinkAccepted(Identifier, _parentId.ValueUnsafe()));
        }

        public async Task RemoveLink(SupplierId parentId, IEventPublisher eventPublisher)
        {
            if (_parentId != parentId)
                throw new NotLinkedWithGivenParent();

            var linkRemoved = new LinkRemoved(Identifier, _parentId.ValueUnsafe());
            _parentId = Prelude.None;
            await eventPublisher.Publish(linkRemoved);
        }

        private bool StatusAllowsToAcceptLink => _status != RelationshipStatus.Accepted;

        private bool StatusAllowsToDeclineLink => _status != RelationshipStatus.Declined;

        private bool IsLinkedWithParent => _parentId.IsSome;

        private void AssertStateIsConsistent()
        {
            if (Identifier == null || _children == null ||
                _parentId.IsNone && _status.IsSome ||
                _parentId.IsSome && _status.IsNone ||
                _children.Any(c => c == _parentId) ||
                _children.Contains(Identifier) ||
                _parentId == Identifier)
                throw new InconsistentState();
        }
    }
}