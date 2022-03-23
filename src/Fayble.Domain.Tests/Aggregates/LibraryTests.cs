using System;
using Fayble.Core.Exceptions;
using Fayble.Domain.Tests.DataBuilders;
using FluentAssertions;
using Xunit;

namespace Fayble.Domain.Tests.Aggregates;

public class LibraryTests
{
    [Fact]
    public void Create_WithEmptyId_ThrowsException()
    {
        Action act = () => LibraryBuilder.WithDefaults().WithId(Guid.Empty).Build();
        act.Should().Throw<DomainException>().WithMessage("Id cannot be empty.");
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsException()
    {
        Action act = () => LibraryBuilder.WithDefaults().WithName(" ").Build();
        act.Should().Throw<DomainException>().WithMessage("Name must be not null and not an empty string.");
    }
}