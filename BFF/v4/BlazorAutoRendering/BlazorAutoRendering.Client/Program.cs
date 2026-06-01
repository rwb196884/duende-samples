// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using BlazorAutoRendering.Api.Proxy;
using BlazorAutoRendering.Client;
using Duende.Bff.Blazor.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddBffBlazorClient(/*opt => opt.RemoteApiPath = "remote-apis/greetings/"*/)
    .AddCascadingAuthenticationState();

builder.Services.AddSingleton<IWeatherClient>(sp => sp.GetRequiredService<WeatherClient>());

builder.Services.AddLocalApiHttpClient<WeatherClient>();

builder.Services.AddRemoteApiHttpClient(MagicValues.Greet, (HttpClient client) =>
{
    client.BaseAddress = new Uri(client.BaseAddress!.ToString() + MagicValues.Greet + "/");
}
);

#region "Greetings API proxy: client side services"
builder.Services.AddRemoteApiHttpClient("IGreetingsApi", (HttpClient client) =>
    {
        client.BaseAddress = new Uri(client.BaseAddress!.ToString() + "IGreetingsApi/");
    }
);
builder.Services.AddSingleton<IGreetingsApi, GreetingsApi>((IServiceProvider serviceProvider) =>
{
    IHttpClientFactory hcf = serviceProvider.GetRequiredService<IHttpClientFactory>();
    HttpClient hc = hcf.CreateClient(nameof(IGreetingsApi));
    return new GreetingsApi(null, hc);
});
#endregion

await builder.Build().RunAsync();
