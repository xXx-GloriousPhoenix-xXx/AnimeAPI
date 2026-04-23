using System.Linq.Expressions;
using AnimeApplication.Application.DTOs.Anime;
using AnimeApplication.Application.Services;
using AnimeApplication.Domain.Entities;
using AnimeApplication.Domain.Interfaces;
using AutoMapper;
using FluentAssertions;
using Moq;

namespace UnitTesting.Services;

public class AnimeServiceTests {
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IBaseRepository<Anime>> _repo;
    private readonly Mock<IMapper> _mapper;
    private readonly AnimeService _sut;

    public AnimeServiceTests() {
        _uow = new Mock<IUnitOfWork>();
        _repo = new Mock<IBaseRepository<Anime>>();
        _mapper = new Mock<IMapper>();

        _uow.Setup(u => u.Animes).Returns(_repo.Object);

        _sut = new AnimeService(_uow.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsMappedDto() {
        // Arrange
        var id = Guid.NewGuid();
        var anime = new Anime { Id = id, Title = "Naruto" };
        var dto = new GetAnimeDTO { Id = id, Title = "Naruto" };

        _repo.Setup(r => r.GetByIdAsync(
            id,
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Anime, object?>>[]>()))
            .ReturnsAsync(anime);
        _mapper.Setup(m => m.Map<GetAnimeDTO>(anime)).Returns(dto);

        // Act
        var result = await _sut.GetByIdAsync(id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull() {
        // Arrange
        _repo.Setup(r => r.GetByIdAsync(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Anime, object?>>[]>()))
            .ReturnsAsync((Anime?)null);

        // Act
        var act = async () => await _sut.GetByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }
}
