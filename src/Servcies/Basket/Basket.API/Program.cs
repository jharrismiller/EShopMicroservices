using bbExp = BuildingBlocks.Exceptions;
using bbBhav = BuildingBlocks.Behaviors;
using Discount.Grpc;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;


var builder = WebApplication.CreateBuilder(args);

#region "Application Services"

var thisAssembly = typeof(Program).Assembly;

var cnBasketDb = builder.Configuration.GetConnectionString("BasketDb")!;

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(thisAssembly);
    config.AddOpenBehavior(typeof(bbBhav.ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(bbBhav.LoggingBehavior<,>));
});

#endregion

#region "Data Services"

builder.Services.AddMarten(config =>
{
    config.Connection(cnBasketDb);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")!;
    //options.InstanceName = "Basket";
});

#endregion

#region "Grpc Services"

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
});

#endregion


#region "Cross-Cutting Services"

builder.Services.AddValidatorsFromAssembly(thisAssembly);
builder.Services.AddExceptionHandler<bbExp.Handler.CustomExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


#endregion

var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler();
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
