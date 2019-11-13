using Ev.Subsidiaries.Domain.Errors;
using Ev.Subsidiaries.Domain.Events;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Ev.Subsidiaries.Domain.UnitTests.Steps
{
    [Binding]
    public class SubsidiariesLinkageSteps
    {
        private readonly EventPublisherMock<IDomainEvent> _eventPublisherMock;
        private readonly Mock<IEventPublisher> _eventPublisherStub = new Mock<IEventPublisher>();

        private Supplier _aggregate;
        private SupplierId _subsidiaryId;
        private SupplierId _parentId;

        private Supplier _spaceX;
        private Supplier _spaceXEurope;
        private Supplier _spaceXPoland;
        private Exception _exception;

        public SubsidiariesLinkageSteps()
        {
            _eventPublisherMock = new EventPublisherMock<IDomainEvent>();
        }

        [Given(@"Two suppliers SpaceX Europe and SpaceX Poland")]
        public void GivenTwoSuppliersSpaceXEuropeAndSpaceXPoland()
        {
            _spaceXEurope = new Supplier(new SupplierId(1));
            _spaceXPoland = new Supplier(new SupplierId(2));
        }

        [When(@"I create pending link for SpaceX Poland as subsidiary of SpaceX Europe")]
        public async Task WhenICreatePendingLinkForSpaceXPolandAsSubsidiaryOfSpaceXEurope()
        {
            await _spaceXPoland.CreatePendingLink(_spaceXEurope.Identifier, _eventPublisherMock.Object);
        }

        [Then(@"Pending link is created for SpaceX Poland as subsidiary of SpaceX Europe")]
        public void ThenPendingLinkIsCreatedForSpaceXPolandAsSubsidiaryOfSpaceXEurope()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<PendingLinkCreated>();
                @event.As<PendingLinkCreated>().SubsidiaryId.Should().Be(_spaceXPoland.Identifier);
                @event.As<PendingLinkCreated>().ParentId.Should().Be(_spaceXEurope.Identifier);
            });
        }

        [When(@"I create accepted link for SpaceX Poland as subsidiary of SpaceX Europe")]
        public async Task WhenICreateAcceptedLinkForSpaceXPolandAsSubsidiaryOfSpaceXEurope()
        {
            await _spaceXPoland.CreateAcceptedLink(_spaceXEurope.Identifier, _eventPublisherMock.Object);
        }

        [Then(@"Accepted link is created for SpaceX Poland as subsidiary of SpaceX Europe")]
        public void ThenAcceptedLinkIsCreatedForSpaceXPolandAsSubsidiaryOfSpaceXEurope()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<AcceptedLinkCreated>();
                @event.As<AcceptedLinkCreated>().SubsidiaryId.Should().Be(_spaceXPoland.Identifier);
                @event.As<AcceptedLinkCreated>().ParentId.Should().Be(_spaceXEurope.Identifier);
            });
        }

        [Given(@"(.*) link between (.*) and (.*)")]
        public async Task GivenStatusAsLinkBetweenAnd(string status, string subsidiary, string parent)
        {
            _subsidiaryId = new SupplierId(1);
            _parentId = new SupplierId(2);

            _aggregate = new Supplier(_subsidiaryId);
            if (status == "Pending")
            {
                await _aggregate.CreatePendingLink(_parentId, _eventPublisherStub.Object);
            }
            else if (status == "Declined")
            {
                await _aggregate.CreatePendingLink(_parentId, _eventPublisherStub.Object);
                await _aggregate.DeclineLink(_parentId, _eventPublisherStub.Object);
            }
            else if (status == "Accepted")
            {
                await _aggregate.CreatePendingLink(_parentId, _eventPublisherStub.Object);
                await _aggregate.AcceptLink(_parentId, _eventPublisherStub.Object);
            }
            else
            {
                throw new ArgumentException(nameof(status));
            }
        }

        [When(@"I accept the link")]
        public async Task WhenIAcceptTheLink()
        {
            await _aggregate.AcceptLink(_parentId, _eventPublisherMock.Object);
        }

        [Then(@"Link is accepted")]
        public void ThenLinkIsAccepted()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<LinkAccepted>();
                @event.As<LinkAccepted>().SubsidiaryId.Should().Be(_subsidiaryId);
                @event.As<LinkAccepted>().ParentId.Should().Be(_parentId);
            });
        }

        [When(@"I decline the link")]
        public async Task WhenIDeclineTheLink()
        {
            await _aggregate.DeclineLink(_parentId, _eventPublisherMock.Object);
        }

        [Then(@"Link is declined")]
        public void ThenLinkIsDeclined()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<LinkDeclined>();
                @event.As<LinkDeclined>().SubsidiaryId.Should().Be(_subsidiaryId);
                @event.As<LinkDeclined>().ParentId.Should().Be(_parentId);
            });
        }

        [When(@"I remove the link")]
        public async Task WhenIRemoveTheLink()
        {
            await _aggregate.RemoveLink(_parentId, _eventPublisherMock.Object);
        }

        [Then(@"Link is removed")]
        public void ThenLinkIsRemoved()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<LinkRemoved>();
                @event.As<LinkRemoved>().SubsidiaryId.Should().Be(_subsidiaryId);
                @event.As<LinkRemoved>().ParentId.Should().Be(_parentId);
            });
        }

        [Given(@"SpaceX Poland as subsidiary of SpaceX Europe")]
        public async Task GivenSpaceXPolandAsSubsidiaryOfSpaceXEurope()
        {
            _spaceXPoland = new Supplier(new SupplierId(1));
            _spaceXEurope = new Supplier(new SupplierId(2), new List<SupplierId> { _spaceXPoland.Identifier });
            await _spaceXPoland.CreatePendingLink(_spaceXEurope.Identifier, _eventPublisherStub.Object);
        }

        [When(@"I create link for SpaceX Europe as subsidiary of SpaceX Poland")]
        public async Task WhenICreateLinkForSpaceXEuropeAsSubsidiaryOfSpaceXPoland()
        {
            try
            {
                await _spaceXEurope.CreatePendingLink(_spaceXPoland.Identifier, _eventPublisherMock.Object);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"Action is denied")]
        public void ThenActionIsDenied()
        {
            _exception.Should().BeOfType<CannotLinkWithChild>();
        }

        [Given(@"SpaceX with subsidiary SpaceX Europe with subsidiary SpaceX Poland")]
        public async Task GivenSpaceXWithSubsidiarySpaceXEuropeWithSubsidiarySpaceXPoland()
        {
            _spaceXPoland = new Supplier(new SupplierId(1));
            _spaceXEurope = new Supplier(new SupplierId(2), new List<SupplierId> { _spaceXPoland.Identifier });
            _spaceX = new Supplier(new SupplierId(3), new List<SupplierId> { _spaceXEurope.Identifier });
            await _spaceXPoland.CreatePendingLink(_spaceXEurope.Identifier, _eventPublisherStub.Object);
            await _spaceXEurope.CreatePendingLink(_spaceX.Identifier, _eventPublisherStub.Object);
        }

        [When(@"I create link for SpaceX as subsidiary of SpaceX Poland")]
        public async Task WhenICreateLinkForSpaceXAsSubsidiaryOfSpaceXPoland()
        {
            await _spaceX.CreatePendingLink(_spaceXPoland.Identifier, _eventPublisherMock.Object);
        }

        [Then(@"Link is created for SpaceX as subsidiary of SpaceX Poland")]
        public void ThenLinkIsCreatedForSpaceXAsSubsidiaryOfSpaceXPoland()
        {
            _eventPublisherMock.VerifyPublishedEvent(@event =>
            {
                @event.Should().BeOfType<PendingLinkCreated>();
                @event.As<PendingLinkCreated>().SubsidiaryId.Should().Be(_spaceX.Identifier);
                @event.As<PendingLinkCreated>().ParentId.Should().Be(_spaceXPoland.Identifier);
            });
        }
    }
}