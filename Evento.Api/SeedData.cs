using Evento.Infrastructure.Services;
using Evento.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Evento.Api;
// namespace Evento.Infrastructure.Settings;

public class SeedData
{
    private readonly IApplicationBuilder _application;

    public SeedData(IApplicationBuilder application) {

        _application = application;
    }

    public void OnSeed() {

        var settings = _application.ApplicationServices
            .GetService<IOptions<AppSettings>>(); 
        //if(settings.Value.SeedData) 
        if(settings.Value.SeedData)
        {
            Console.WriteLine($"AppSettings is true: {settings.Value.SeedData}");

            /*  var dataInitializer = _application.ApplicationServices.GetService<IDataInitializer>();            
            dataInitializer.SeedAsync(); */
        }
    }
}