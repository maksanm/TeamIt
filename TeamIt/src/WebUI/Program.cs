using Blazorise;
using Blazorise.Icons.Material;
using Blazorise.Material;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using WebUI;
using WebUI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient

var apiUrl = builder.Configuration["apiUrl"]!;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });

// Own services

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<IHttpService, HttpService>()
    .AddScoped<ILocalStorageService, LocalStorageService>()
    .AddScoped<ITeamService, TeamService>()
    .AddScoped<IPermissionService, PermissionService>()
    .AddScoped<IProjectService, ProjectService>()
    .AddScoped<IChatService, ChatService>();

builder.Services.AddSingleton(sp => {
    return new HubConnectionBuilder()
      .WithUrl(apiUrl + "chatHub")
      .WithAutomaticReconnect()
      .Build();
});

// Blazorise

builder.Services
        .AddBlazorise();
builder.Services
    .AddMaterialProviders()
    .AddMaterialIcons();

// Auto-login

var host = builder.Build();
var accountService = host.Services.GetRequiredService<IUserService>();
await accountService.Initialize();
var httpService = host.Services.GetRequiredService<IHttpService>();
httpService.ApiUrl = apiUrl;

await host.RunAsync();