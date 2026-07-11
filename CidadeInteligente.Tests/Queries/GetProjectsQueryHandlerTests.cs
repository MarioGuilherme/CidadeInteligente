using CidadeInteligente.Application.Options;
using CidadeInteligente.Application.Queries.GetProjects;
using CidadeInteligente.Domain.Common;
using CidadeInteligente.Domain.Entities;
using CidadeInteligente.Domain.Repositories;
using CidadeInteligente.Domain.Specifications;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace CidadeInteligente.Tests.Queries;

public class GetProjectsQueryHandlerTests
{
    private const int PageSize = 6;

    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly GetProjectsQueryHandler _queryHandler;

    public GetProjectsQueryHandlerTests()
        => _queryHandler = new(_unitOfWork.Object,
            Options.Create(new FileStorageOptions { BaseUrl = "https://storage.cidadeinteligente.com" }),
            Options.Create(new PaginationOptions { MaxPageSize = PageSize }));

    [Fact(DisplayName = "Should return paged projects with medias")]
    public async Task Should_ReturnPagedProjectsWithMedias()
    {
        // Arrange
        GetProjectsQuery getProjectsQuery = new(2);
        List<GetProjectsQueryResult.ProjectViewModel> projects = [
            new(1, "Projeto A", null, [new(1, "https://storage.cidadeinteligente.com/file-1", "image/png")]),
            new(2, "Projeto B", "Descrição", [new(2, "https://storage.cidadeinteligente.com/file-2", "video/mp4")])];
        PagedResult<GetProjectsQueryResult.ProjectViewModel> pagedResult = new(projects, totalItems: 8, page: 2, pageSize: PageSize);
        _unitOfWork
            .Setup(u => u.Projects.GetPagedBySpecAsync(It.IsAny<Specification<Project, GetProjectsQueryResult.ProjectViewModel>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        // Act
        GetProjectsQueryResult result = await _queryHandler.Handle(getProjectsQuery, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(2);
        result.TotalPages.Should().Be(2);
        result.ItemsCount.Should().Be(8);
        result.Data.Should().BeEquivalentTo(projects);
    }

    [Fact(DisplayName = "Should apply requested page and configured page size to the specification")]
    public async Task Should_ApplyRequestedPageAndConfiguredPageSize()
    {
        // Arrange
        GetProjectsQuery getProjectsQuery = new(3);
        Specification<Project, GetProjectsQueryResult.ProjectViewModel>? capturedSpec = null;
        _unitOfWork
            .Setup(u => u.Projects.GetPagedBySpecAsync(It.IsAny<Specification<Project, GetProjectsQueryResult.ProjectViewModel>>(), It.IsAny<CancellationToken>()))
            .Callback<Specification<Project, GetProjectsQueryResult.ProjectViewModel>, CancellationToken>((spec, _) => capturedSpec = spec)
            .ReturnsAsync(new PagedResult<GetProjectsQueryResult.ProjectViewModel>([], 0, 3, PageSize));

        // Act
        await _queryHandler.Handle(getProjectsQuery, CancellationToken.None);

        // Assert
        capturedSpec.Should().NotBeNull();
        capturedSpec.Query.IsPagingEnabled.Should().BeTrue();
        capturedSpec.Query.Page.Should().Be(3);
        capturedSpec.Query.PageSize.Should().Be(PageSize);
    }
}
