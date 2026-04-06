using Anime.BLL.DTO.Extra;
using Anime.BLL.DTO.Waifu;
using FluentAssertions;
using System.Net;

namespace IntegrationTests;
public class WaifuIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetAll_ReturnsOkWithItems()
    {
        var response = await _client.GetAsync("/api/waifus", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var page = await response.Content.ReadFromJsonAsync<Page<GetFullWaifuDTO>>(cancellationToken: TestContext.Current.CancellationToken);
        page!.Items.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsOkWithDto()
    {
        var response = await _client.GetAsync($"/api/waifus/{CustomWebApplicationFactory.ExistingWaifuId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetFullWaifuDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Name.Should().Be("Hinata");
    }

    [Fact]
    public async Task GetById_UnknownId_Returns500()
    {
        var response = await _client.GetAsync($"/api/waifus/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddWithAnimeId_ValidDto_ReturnsOkWithDto()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Sakura"), "Name" },
            { new StringContent("Haruno"), "Surname" },
            { new StringContent("17"), "Age" },
            { new StringContent(CustomWebApplicationFactory.ExistingAnimeId.ToString()), "AnimeId" }
        };

        var response = await _client.PostAsync("/api/waifus/id", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetFullWaifuDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Name.Should().Be("Sakura");
    }

    [Fact]
    public async Task AddWithAnimeId_UnknownAnimeId_Returns500()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Sakura"), "Name" },
            { new StringContent("Haruno"), "Surname" },
            { new StringContent("17"), "Age" },
            { new StringContent(Guid.NewGuid().ToString()), "AnimeId" }
        };

        var response = await _client.PostAsync("/api/waifus/id", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task AddWithAnimeName_ValidName_ReturnsOkWithDto()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Ino"), "Name" },
            { new StringContent("Yamanaka"), "Surname" },
            { new StringContent("17"), "Age" },
            { new StringContent("Naruto"), "AnimeTitle" }
        };

        var response = await _client.PostAsync("/api/waifus/name", form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetFullWaifuDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Name.Should().Be("Ino");
    }

    [Fact]
    public async Task Update_ExistingId_ReturnsOkWithUpdatedDto()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Hinata Updated"), "Name" }
        };

        var response = await _client.PatchAsync($"/api/waifus/{CustomWebApplicationFactory.ExistingWaifuId}",
            form, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var dto = await response.Content.ReadFromJsonAsync<GetFullWaifuDTO>(cancellationToken: TestContext.Current.CancellationToken);
        dto!.Name.Should().Be("Hinata Updated");
    }

    [Fact]
    public async Task Delete_Force_ExistingId_ReturnsOk()
    {
        var form = new MultipartFormDataContent
        {
            { new StringContent("Temp"), "Name" },
            { new StringContent("Waifu"), "Surname" },
            { new StringContent("18"), "Age" },
            { new StringContent(CustomWebApplicationFactory.ExistingAnimeId.ToString()), "AnimeId" }
        };
        var created = await (await _client.PostAsync("/api/waifus/id", form, TestContext.Current.CancellationToken))
            .Content.ReadFromJsonAsync<GetFullWaifuDTO>(cancellationToken: TestContext.Current.CancellationToken);

        var response = await _client.DeleteAsync($"/api/waifus/{created!.Id}?isSoft=false", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
