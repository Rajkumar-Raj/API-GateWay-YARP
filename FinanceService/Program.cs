var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/finance", () => "Finance Service is up and running!");
app.Run();


