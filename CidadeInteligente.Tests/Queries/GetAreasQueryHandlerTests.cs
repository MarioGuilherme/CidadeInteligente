using CidadeInteligente.Application.Queries.GetAreas;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class GetAreasQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetAreasQueryHandler _queryHandler;

    public GetAreasQueryHandlerTests()
        => _queryHandler = new(_unitOfWork.Object);

    [Fact(DisplayName = "Should return all areas")]
    public async Task Should_ReturnAllAreas()
    {
        // Arrange
        List<GetAreasQueryResult.AreaViewModel> areas = [new(1, "Meio Ambiente"), new(2, "Mobilidade Urbana")];
        _unitOfWork
            .Setup(u => u.Areas.GetAllBySpecAsync(It.IsAny<Specification<Area, GetAreasQueryResult.AreaViewModel>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(areas);

        // Act
        GetAreasQueryResult result = await _queryHandler.Handle(new(), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Areas.Should().HaveCount(2);
        result.Areas.Should().BeEquivalentTo(areas);
    }

}
