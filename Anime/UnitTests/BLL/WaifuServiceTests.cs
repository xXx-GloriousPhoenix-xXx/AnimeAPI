using Anime.BLL.DTO.Waifu;
using Anime.BLL.Service.Implementation;
using Anime.DAL.Entity;
using Anime.DAL.Repository.Interface;
using AutoMapper;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace UnitTests.BLL;
public class WaifuServiceTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IBaseRepository<Waifu>> _waifuRepo;
    private readonly Mock<IBaseRepository<AnimeEntity>> _animeRepo;
    private readonly Mock<IMapper> _mapper;
    private readonly WaifuService _sut;

    public WaifuServiceTests()
    {
        _uow = new Mock<IUnitOfWork>();
        _waifuRepo = new Mock<IBaseRepository<Waifu>>();
        _animeRepo = new Mock<IBaseRepository<AnimeEntity>>();
        _mapper = new Mock<IMapper>();

        _uow.Setup(u => u.Waifus).Returns(_waifuRepo.Object);
        _uow.Setup(u => u.Animes).Returns(_animeRepo.Object);

        _sut = new WaifuService(_uow.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull()
    {
        // Arrange
        _waifuRepo.Setup(r => r.GetByIdAsync(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<Waifu, object?>>[]>()))
            .ReturnsAsync((Waifu?)null);

        // Act
        var act = async () => await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddWithAnimeIdAsync_AnimeNotFound_ReturnsNull()
    {
        // Arrange
        var dto = new CreateWaifuWithAnimeIdDTO { AnimeId = Guid.NewGuid() };

        _animeRepo.Setup(r => r.GetByIdAsync(
            dto.AnimeId,
            It.IsAny<CancellationToken>(),
            It.IsAny<Expression<Func<AnimeEntity, object?>>[]>()))
            .ReturnsAsync((AnimeEntity?)null);

        // Act
        var act = async () => await _sut.AddWithAnimeIdAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _waifuRepo.Verify(r => r.Add(It.IsAny<Waifu>()), Times.Never);
    }

    [Fact]
    public async Task AddWithAnimeIdAsync_ValidAnime_AddsWaifuAndReturnsDto()
    {
        // Arrange
        var animeId = Guid.NewGuid();
        var anime = new AnimeEntity { Id = animeId, Title = "SAO" };
        var dto = new CreateWaifuWithAnimeIdDTO { AnimeId = animeId, Name = "Asuna" };
        var waifu = new Waifu { Id = Guid.NewGuid(), Name = "Asuna", AnimeId = animeId };
        var getDto = new GetFullWaifuDTO { Name = "Asuna" };

        _animeRepo
            .Setup(r => r.GetByIdAsync(animeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(anime);

        _mapper
            .Setup(m => m.Map<Waifu>(dto))
            .Returns(waifu);

        _uow
            .Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _waifuRepo
            .Setup(r => r.GetByIdAsync(waifu.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(waifu);

        _mapper
            .Setup(m => m.Map<GetFullWaifuDTO>(waifu))
            .Returns(getDto);


        // Act
        var act = await _sut.AddWithAnimeIdAsync(dto, TestContext.Current.CancellationToken);

        // Assert
        _waifuRepo.Verify(r => r.Add(waifu), Times.Once);
        act.Should().BeEquivalentTo(getDto);
    }

    [Fact]
    public async Task AddWithAnimeNameAsync_AnimeNotFound_ReturnsNull()
    {
        // Arrange
        var dto = new CreateWaifuWithAnimeNameDTO { AnimeTitle = "Unknown Anime" };

        _animeRepo
            .Setup(r => r.FindAsync(
                It.IsAny<Expression<Func<AnimeEntity, bool>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var act = async () => await _sut.AddWithAnimeNameAsync(dto, TestContext.Current.CancellationToken);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SoftDeleteAsync_Existing_SetsIsDeletedTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var waifu = new Waifu { Id = id, IsDeleted = false };

        _waifuRepo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                  .ReturnsAsync(waifu);
        _uow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        await _sut.SoftDeleteAsync(id, TestContext.Current.CancellationToken);

        // Assert
        waifu.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task ForceDeleteAsync_Existing_CallsDeleteOnRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var waifu = new Waifu { Id = id };

        _waifuRepo.Setup(r => r.GetByIdAsync(
            id,
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(waifu);
        _uow.Setup(u => u.CompleteAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        // Act
        await _sut.ForceDeleteAsync(id, TestContext.Current.CancellationToken);

        // Assert
        _waifuRepo.Verify(r => r.Delete(waifu), Times.Once);
    }
}
