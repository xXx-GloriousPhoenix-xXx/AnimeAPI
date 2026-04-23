using AnimeApplication.Domain.Entities;
using AnimeApplication.Infrastructure.Repositories;
using AnimeApplication.Infrastructure.Context;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace UnitTesting.Repositories;

public class BaseRepositoryTests : IAsyncLifetime {
    private AnimeDbContext _context = null!;
    private BaseRepository<Anime> _repo = null!;

    public async ValueTask InitializeAsync() {
        var options = new DbContextOptionsBuilder<AnimeDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AnimeDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _repo = new BaseRepository<Anime>(_context);
    }

    public async ValueTask DisposeAsync() {
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task Add_ThenGetById_ReturnsEntity() {
        // Arrange
        var anime = new Anime { Id = Guid.NewGuid(), Title = "Naruto", EpisodeCount = 220 };

        // Act
        _repo.Add(anime);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _repo.GetByIdAsync(anime.Id, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Naruto");
    }

    [Fact]
    public async Task GetByIdAsync_NotFound_ReturnsNull() {
        // Arrange
        // ...

        // Act
        var result = await _repo.GetByIdAsync(Guid.NewGuid(), TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities() {
        // Arrange
        var animes = Enumerable.Range(1, 5)
            .Select(i => new Anime { Id = Guid.NewGuid(), Title = $"Anime {i}" })
            .ToList();

        // Act
        animes.ForEach(_repo.Add);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = await _repo.GetAllAsync(TestContext.Current.CancellationToken);

        // Assert
        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task FindAsync_MatchingPredicate_ReturnsFiltered() {
        // Arrange
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "Naruto", EpisodeCount = 220 });
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "Bleach", EpisodeCount = 366 });
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "One Piece", EpisodeCount = 1000 });
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.FindAsync(a => a.EpisodeCount > 300, TestContext.Current.CancellationToken);

        // Assert
        result.Should().HaveCount(2);
        result.Select(a => a.Title).Should().Contain(["Bleach", "One Piece"]);
    }

    [Fact]
    public async Task FindAsync_NoMatch_ReturnsEmpty() {
        // Arrange
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "Naruto", EpisodeCount = 220 });
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.FindAsync(a => a.EpisodeCount > 9999, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ExistsAsync_Matching_ReturnsTrue() {
        // Arrange
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "Naruto" });
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.ExistsAsync(a => a.Title == "Naruto", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_NotMatching_ReturnsFalse() {
        // Arrange
        // ...

        // Act
        var result = await _repo.ExistsAsync(a => a.Title == "Unknown", TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CountAsync_NoPredicate_ReturnsTotal() {
        // Arrange
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "A" });
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "B" });
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.CountAsync(ct: TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ReturnsFiltered() {
        // Arrange
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "A", EpisodeCount = 10 });
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "B", EpisodeCount = 20 });
        _repo.Add(new Anime { Id = Guid.NewGuid(), Title = "C", EpisodeCount = 30 });
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.CountAsync(a => a.EpisodeCount >= 20, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task Update_ChangesArePersisted() {
        // Arrange
        var anime = new Anime { Id = Guid.NewGuid(), Title = "Old Title" };
        _repo.Add(anime);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        anime.Title = "New Title";
        _repo.Update(anime);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var result = await _repo.GetByIdAsync(anime.Id, TestContext.Current.CancellationToken);
        result!.Title.Should().Be("New Title");
    }

    [Fact]
    public async Task Delete_EntityIsRemoved() {
        // Arrange
        var anime = new Anime { Id = Guid.NewGuid(), Title = "To Delete" };
        _repo.Add(anime);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        _repo.Delete(anime);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Assert
        var result = await _repo.GetByIdAsync(anime.Id, TestContext.Current.CancellationToken);
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WithIncludes_LoadsNavigation() {
        // Arrange
        var anime = new Anime { Id = Guid.NewGuid(), Title = "SAO" };
        var waifu = new Waifu {
            Id = Guid.NewGuid(),
            Name = "Asuna",
            AnimeId = anime.Id
        };

        _context.Set<Anime>().Add(anime);
        _context.Set<Waifu>().Add(waifu);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        // Act
        var result = await _repo.GetByIdAsync(anime.Id, TestContext.Current.CancellationToken, [a => a.Waifus]);

        // Assert
        result.Should().NotBeNull();
        result!.Waifus.Should().HaveCount(1);
        result.Waifus.First().Name.Should().Be("Asuna");
    }

    [Fact]
    public void AsQueryable_ReturnsQueryableOfCorrectType() {
        // Arrange and Act
        var queryable = _repo.AsQueryable();

        // Assert
        queryable.Should().BeAssignableTo<IQueryable<Anime>>();
    }
}
