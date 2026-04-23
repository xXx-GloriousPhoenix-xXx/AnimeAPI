using AnimeApplication.API.Controllers;
using AnimeApplication.Application.Common;
using AnimeApplication.Application.DTOs.Anime;
using AnimeApplication.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.Controllers;

public class AnimeControllerTests {
    private readonly Mock<IAnimeService> _service;
    private readonly AnimeController _sut;

    public AnimeControllerTests() {
        _service = new Mock<IAnimeService>();
        _sut = new AnimeController(_service.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithPage() {
        // Arrange
        var page = new Page<GetAnimeDTO>();
        _service.Setup(s => s.GetAllAsync(10, 1, TestContext.Current.CancellationToken)).ReturnsAsync(page);

        // Act
        var act = await _sut.GetAllAsync(10, 1, TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        act!.StatusCode.Should().Be(200);
        act.Value.Should().BeEquivalentTo(page);
    }

    [Fact]
    public async Task GetById_ReturnsOkWithDto() {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new GetAnimeDTO { Id = id, Title = "One Piece" };
        _service.Setup(s => s.GetByIdAsync(id, TestContext.Current.CancellationToken)).ReturnsAsync(dto);

        // Act
        var act = await _sut.GetByIdAsync(id, TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        act!.StatusCode.Should().Be(200);
        act.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task GetById_NotFound_ReturnsOkWithNull() {
        // Arrange
        _service.Setup(s => s.GetByIdAsync(
            It.IsAny<Guid>(), default))
            .ReturnsAsync((GetAnimeDTO?)null);

        // Act
        var act = await _sut.GetByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        act!.Value.Should().BeNull();
    }

    [Fact]
    public async Task Add_ReturnsOkWithDto() {
        // Arrange
        var createDto = new CreateAnimeDTO { Title = "HxH" };
        var getDto = new GetAnimeDTO { Title = "HxH" };
        _service.Setup(s => s.AddAsync(createDto, TestContext.Current.CancellationToken)).ReturnsAsync(getDto);

        // Act
        var act = await _sut.AddAsync(createDto, TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        act!.StatusCode.Should().Be(200);
        act.Value.Should().BeEquivalentTo(getDto);
    }

    [Fact]
    public async Task Delete_SoftTrue_CallsSoftDelete() {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _sut.DeleteAsync(id, isSoft: true, ct: TestContext.Current.CancellationToken);

        // Assert
        _service.Verify(s => s.SoftDeleteAsync(id, TestContext.Current.CancellationToken), Times.Once);
        _service.Verify(s => s.ForceDeleteAsync(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Delete_SoftFalse_CallsForceDelete() {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _sut.DeleteAsync(id, isSoft: false, ct: TestContext.Current.CancellationToken);

        // Assert
        _service.Verify(s => s.ForceDeleteAsync(id, TestContext.Current.CancellationToken), Times.Once);
        _service.Verify(s => s.SoftDeleteAsync(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
