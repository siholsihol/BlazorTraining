using BatchAndExcel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices(option =>
{
    option.R_DisableBlazorContext();
    option.R_DisableCrossPlatformSecurity();
    option.R_DisableAuthentication();
});

var host = builder.Build();

host.R_SetupBlazorService();

await host.RunAsync();
