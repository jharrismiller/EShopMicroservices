using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var thisAssembly = typeof(Program).Assembly;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(thisAssembly);
    //config.AddOpenBehavior(typeof(bb.Behaviors.ValidationBehavior<,>));
    //config.AddOpenBehavior(typeof(bb.Behaviors.LoggingBehavior<,>));
});

var app = builder.Build();

app.MapCarter();

app.Run();
