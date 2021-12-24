var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTypedCache();

var app = builder.Build();

app.MapGet("/", () => "Welcome to your mini-api!");

app.MapEndpoints("/weather", (router) =>
{
  var get = (string? city) => Results.NotFound(city is null ? "No weather found" : $"No weather found for {city}");

  router.MapGet("", get);
  router.MapGet("{city}", get);
});

app.MapEndpoints("/todo", (router) => router.MapCachedRest<Todo>(t => t.Id, "todo"));
app.MapEndpoints("/person", (router) => router.MapCachedRest<Person>(t => t.Id, "person"));
app.MapEndpoints("/book", (router) => router.MapCachedRest<Book>(t => t.Id, "book"));

var port = app.Configuration["PORT"] ?? "3000";
app.Run($"http://*:{port}");
