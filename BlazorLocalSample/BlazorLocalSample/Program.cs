using BlazorLocalSample;
using BlazorLocalSample.Components;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    //.AddInteractiveWebAssemblyComponents()
    ;
builder.Services.AddScoped<ConnectedUser>();
builder.Services.AddSingleton<ConnectedUserList>();
builder.Services.AddHostedService<UserCheckBackgroundService>();
builder.Services.AddAntDesign();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}


app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
