using AutoFixture.Xunit2;
using Ev.Subsidiaries.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static LanguageExt.Prelude;

namespace Ev.Subsidiaries.Domain.UnitTests
{
    public class ConstructingSupplier
    {
        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithItselfAsChild(SupplierId subsidiaryId)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, new List<SupplierId>{subsidiaryId}));

        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithItselfAsParent(SupplierId subsidiaryId, RelationshipStatus relationshipStatus, List<SupplierId> children)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, parentId: subsidiaryId, relationshipStatus, children));

        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithParentAsChild(SupplierId subsidiaryId, SupplierId parentId, RelationshipStatus relationshipStatus)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, parentId, relationshipStatus, new List<SupplierId>{parentId}));

        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithParentButWithoutRelationshipStatus(SupplierId subsidiaryId, SupplierId parentId, List<SupplierId> children)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, parentId, relationshipStatus: None, children));

        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithRelationshipStatusButWithoutParent(SupplierId subsidiaryId, RelationshipStatus relationshipStatus, List<SupplierId> children)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, parentId: None, relationshipStatus, children));

        [Theory]
        [AutoData]
        public void CannotCreateSupplierWithChildrenAsNull(SupplierId subsidiaryId, SupplierId parentId, RelationshipStatus relationshipStatus)
            => Assert.Throws<InconsistentState>(() => new Supplier(subsidiaryId, parentId, relationshipStatus, children: null));

        [Fact]
        public void CannotCreateSupplierWithoutItsId()
            => Assert.ThrowsAny<Exception>(() => new Supplier(null));

        [Theory]
        [AutoData]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CanCreateSupplierWithCorrectArguments(SupplierId supplierId, SupplierId parentId, RelationshipStatus relationshipStatus, List<SupplierId> children)
            => new Supplier(supplierId, parentId, relationshipStatus, children);
    }
}
