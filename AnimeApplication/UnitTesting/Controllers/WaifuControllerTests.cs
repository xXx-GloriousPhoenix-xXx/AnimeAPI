using AnimeApplication.API.Controllers;
using AnimeApplication.Application.Common;
using AnimeApplication.Application.DTOs.Waifu;
using AnimeApplication.Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTesting.Controllers;

public class WaifuControllerTests {
    private readonly Mock<IWaifuService> _service;
    private readonly WaifuController _sut;

    public WaifuControllerTests() {
        _service = new Mock<IWaifuService>();
        _sut = new WaifuController(_service.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithPage() {
        // Arrange
        var page = new Page<GetFullWaifuDTO>();
        _service.Setup(s => s.GetAllAsync(10, 1, default)).ReturnsAsync(page);

        // Act
        var act = await _sut.GetAllAsync(ct: TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        act!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task AddWithId_ReturnsOkWithDto() {
        // Arrange
        var dto = new CreateWaifuWithAnimeIdDTO { Name = "Asuna" };
        var getDto = new GetFullWaifuDTO { Name = "Asuna" };
        _service.Setup(s => s.AddWithAnimeIdAsync(dto, TestContext.Current.CancellationToken)).ReturnsAsync(getDto);

        // Act
        var result = await _sut.AddWithAnimeIdAsync(dto, TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(getDto);
    }

    [Fact]
    public async Task AddWithName_ReturnsOkWithDto() {
        // Arrange
        var dto = new CreateWaifuWithAnimeNameDTO { AnimeTitle = "SAO" };
        var getDto = new GetFullWaifuDTO { Name = "Asuna" };
        _service.Setup(s => s.AddWithAnimeNameAsync(dto, TestContext.Current.CancellationToken)).ReturnsAsync(getDto);

        // Act
        var result = await _sut.AddWithAnimeNameAsync(dto, TestContext.Current.CancellationToken) as OkObjectResult;

        // Assert
        result!.Value.Should().BeEquivalentTo(getDto);
    }

    [Fact]
    public async Task Delete_SoftTrue_CallsSoftDelete() {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _sut.DeleteAsync(id, isSoft: true, ct: TestContext.Current.CancellationToken);

        // Assert
        _service.Verify(s => s.SoftDeleteAsync(id, TestContext.Current.CancellationToken), Times.Once);
    }

    [Fact]
    public async Task Delete_SoftFalse_CallsForceDelete() {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _sut.DeleteAsync(id, isSoft: false, ct: TestContext.Current.CancellationToken);

        // Assert
        _service.Verify(s => s.ForceDeleteAsync(id, TestContext.Current.CancellationToken), Times.Once);
    }
}