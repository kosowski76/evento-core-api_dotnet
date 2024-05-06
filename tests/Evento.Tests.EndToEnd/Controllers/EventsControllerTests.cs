using Evento.Api;
using Evento.Infrastructure.DTO;

namespace Evento.Tests.EndToEnd.Controllers;

public class EventsControllerTests
{
    private readonly TestServer _server;
    private readonly HttpClient _client;

    public EventsControllerTests() {

        // Arrange
        _server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task fetching_events_should_return_not_empty_collection() {

        var response = await _client.GetAsync("events");
        var content = await response.Content.ReadAsStringAsync();
        var events = JsonConvert.DeserializeObject<IEnumerable<EventDto>>(content);

        response.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);
        events.Should().NotBeEmpty();
    }
}
