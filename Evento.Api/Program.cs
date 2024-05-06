using Evento.Api;
using Evento.Api.Exceptions;
using Evento.Core.Repositories;
using Evento.Infrastructure.Mappers;
using Evento.Infrastructure.Repositories;
using Evento.Infrastructure.Services;
using Evento.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var jwtSettings = builder.Configuration.GetSection(JwtSettings.Jwt);
builder.Services.Configure<JwtSettings>(jwtSettings);
var appSettings = builder.Configuration.GetSection(AppSettings.App);
builder.Services.Configure<AppSettings>(appSettings);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option => 
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Key")))
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HasAdminRole", policy =>
        policy.RequireRole("admin"));
});

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService,TicketService>();
builder.Services.AddScoped<IDataInitializer,DataInitializer>();
builder.Services.AddSingleton<IJwtHandler, JwtHandler>();
builder.Services.AddSingleton(AutoMapperConfig.Initialize());

// services.AddScoped<ErrorHandlingMiddleware>();

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddExceptionHandler<GeneralExceptionHandler>();

builder.Services.AddControllers(options => 
{
    options.RespectBrowserAcceptHeader = true;
});

/* builder.Host.ConfigureLogging(logging => {
    logging.ClearProviders();
    logging.AddConsole();
}); */
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddMemoryCache();


var app = builder.Build();

// app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseExceptionHandler(_ => {});

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.Logger.LogInformation("Adding Routes");
app.MapControllers();


app.MapGet("/trow", (_) => throw new Exception());
app.MapGet("/throwNotFound", (_) => throw new DllNotFoundException(".. not found .."));
app.MapGet("/throwNotSupport", (_) => throw new NotSupportedException(".. not support .."));


var seedData = new SeedData(app);
seedData.OnSeed();



app.Run();

