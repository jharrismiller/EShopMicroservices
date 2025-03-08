using bbExp = BuildingBlocks.Exceptions;
using bbBhav = BuildingBlocks.Behaviors;


var builder = WebApplication.CreateBuilder(args);

var thisAssembly = typeof(Program).Assembly;

var cnBasketDb = builder.Configuration.GetConnectionString("BasketDb")!;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(thisAssembly);
    config.AddOpenBehavior(typeof(bbBhav.ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(bbBhav.LoggingBehavior<,>));
});

builder.Services.AddMarten(config =>
{
    config.Connection(cnBasketDb);
}).UseLightweightSessions();

builder.Services.AddValidatorsFromAssembly(thisAssembly);
builder.Services.AddExceptionHandler<bbExp.Handler.CustomExceptionHandler>();
builder.Services.AddProblemDetails();
//builder.Services.AddHealthChecks();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler();
//app.UseHealthChecks("/health",
//    new HealthCheckOptions
//    {
//        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//    });


app.Run();
