using Catalog.API.Data;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using bb = BuildingBlocks;

var builder = WebApplication.CreateBuilder(args);

var thisAssembly = typeof(Program).Assembly;

var cnCatalog = builder.Configuration.GetConnectionString("CatalogDb")!;

//Add Services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(thisAssembly);
    config.AddOpenBehavior(typeof(bb.Behaviors.ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(bb.Behaviors.LoggingBehavior<,>));
});
builder.Services.AddMarten(config =>
{
    config.Connection(cnCatalog);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddValidatorsFromAssembly(thisAssembly);
builder.Services.AddExceptionHandler<bb.Exceptions.Handler.CustomExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
    .AddNpgSql(cnCatalog);

var app = builder.Build();

//Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();