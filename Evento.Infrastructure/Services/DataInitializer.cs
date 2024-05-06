using Microsoft.Extensions.Logging;

namespace Evento.Infrastructure.Services;

public class DataInitializer : IDataInitializer
{
    private readonly ILogger _logger;
    // private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public DataInitializer(
        IUserService userService, 
        IEventService eventService,
        ILogger<EventService> logger) {

        _userService = userService;
        _eventService = eventService;
        _logger = logger; }

    public async Task SeedAsync() {
        
        _logger.LogInformation("Initializing data...");
        // Logger.Info("Initializing data...");

        var tasks = new List<Task>();
        tasks.Add(_userService.RegisterAsync(
            Guid.NewGuid(), "user@email.com", "default user", "secret"));
        tasks.Add(_userService.RegisterAsync(
            Guid.NewGuid(), "admin@email.com", "default user", "secret", "admin"));

        _logger.LogInformation("Created users: user, admin");
        // Logger.Info("Created users: user, admin");

        for(var i = 1; i <= 10; i++)
        {
            var eventId = Guid.NewGuid();
            var name = $"Event {i}";
            var description = $"{name} description.";
            var startDate = DateTime.UtcNow.AddHours(3);
            var endDate = startDate.AddHours(2);

            tasks.Add(_eventService.CreateAsync(
                eventId, name, description, startDate, endDate));
            tasks.Add(_eventService.AddTicketsAsync(
                eventId, 1000, 100));

            _logger.LogInformation($"Created event: {name}");
            // Logger.Info($"Created event: {name}");
        }

        await Task.WhenAll(tasks);

        _logger.LogInformation("Data was initialized.");
        // Logger.Info("Data was initialized.");
    }
}
