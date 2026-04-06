using Anime.BLL.DTO.Anime;
using Anime.BLL.DTO.Extra;
using FluentAssertions;
using System.Net;

namespace IntegrationTests;
public class AnimeIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_ReturnsOkWithItems()
    {
        var response = await _client.GetAsync("/api/animes", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<Page<GetAnimeDTO>>(cancellationToken: TestContext.Current.CancellationToken);
        page.Should().NotBeNull();
        page.Items.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    public async Task GetAll_Pagination_ReturnsCorrectPage(int pageSize, int pageNum)
    {
        var response = await _client.GetAsync($"/api/animes?pageSize={pageSize}&pageNum={pageNum}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<Page<GetAnimeDTO>>(cancellationToken: TestContext.Current.CancellationToken);
        page!.Items.Count().Should().BeLessThanOrEqualTo(pageSize);
        page.PageNum.Should().Be(pageNum);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOkWithDto()
    {
        var response = await _client.GetAsync($"/api/animes/{CustomWebApplicationFactory.ExistingAnimeId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetAnimeDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto.Should().NotBeNull();
        dto!.Title.Should().Be("Naruto");
    }

    [Fact]
    public async Task GetById_UnknownId_Returns500()
    {
        var response = await _client.GetAsync($"/api/animes/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Add_ValidDto_ReturnsOkWithCreatedAnime()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Bleach"),  "Title" },
            { new StringContent("366"),     "EpisodeCount" },
            { new StringContent("2004-10-05"), "ReleaseDate" }
        };

        var response = await _client.PostAsync("/api/animes", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetAnimeDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Title.Should().Be("Bleach");
    }

    [Fact]
    public async Task Update_ExistingId_ReturnsOkWithUpdatedAnime()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Naruto Shippuden"), "Title" }
        };

        var response = await _client.PatchAsync($"/api/animes/{CustomWebApplicationFactory.ExistingAnimeId}", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetAnimeDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Title.Should().Be("Naruto Shippuden");
    }

    [Fact]
    public async Task Delete_Soft_ReturnsOk()
    {
        var animeId = Guid.NewGuid();

        var form = new MultipartFormDataContent
        {
            { new StringContent("Temp Anime"), "Title" },
            { new StringContent("12"), "EpisodeCount" },
            { new StringContent("2020-01-01"), "ReleaseDate" }
        };

        var created = await (await _client.PostAsync("/api/animes", form, TestContext.Current.CancellationToken))
            .Content.ReadFromJsonAsync<GetAnimeDTO>(cancellationToken: TestContext.Current.CancellationToken);

        created.Should().NotBeNull();
        created!.Id.Should().NotBe(Guid.Empty);

        var response = await _client.DeleteAsync($"/api/animes/{created!.Id}?isSoft=true", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_Force_ReturnsOk()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("To Force Delete"), "Title" },
            { new StringContent("1"), "EpisodeCount" },
            { new StringContent("2021-01-01"), "ReleaseDate" }
        };
        var created = await (await _client.PostAsync("/api/animes", form, TestContext.Current.CancellationToken))
            .Content.ReadFromJsonAsync<GetAnimeDTO>(cancellationToken: TestContext.Current.CancellationToken);

        var response = await _client.DeleteAsync($"/api/animes/{created!.Id}?isSoft=false", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
