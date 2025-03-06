var builder = WebApplication.CreateBuilder(args);

//Add Services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("CatalogDb")!);
}).UseLightweightSessions();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

var app = builder.Build();

//Configure the HTTP request pipeline
app.MapCarter();

app.Run();
