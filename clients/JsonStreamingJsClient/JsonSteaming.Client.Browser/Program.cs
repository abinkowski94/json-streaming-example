var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseStaticFiles();

app.Map("/", () => Results.Redirect("~/index.html"));

app.Run();