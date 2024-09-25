var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/inventory", () => "Inventory Service is up and running!");

app.Run();