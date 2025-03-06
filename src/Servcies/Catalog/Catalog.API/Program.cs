using bb = BuildingBlocks.Behaviors;

var builder = WebApplication.CreateBuilder(args);

var thisAssembly = typeof(Program).Assembly;

//Add Services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(thisAssembly);
    config.AddOpenBehavior(typeof(bb.ValidationBehavior<,>));
});
builder.Services.AddMarten(config =>
{
    config.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();
builder.Services.AddValidatorsFromAssembly(thisAssembly);

var app = builder.Build();

//Configure the HTTP request pipeline
app.MapCarter();

app.Run();
