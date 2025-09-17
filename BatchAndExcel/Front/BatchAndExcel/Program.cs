using BatchAndExcel;
using BatchAndExcel.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorFrontEnd.Interfaces;
using R_BlazorFrontEnd.Report;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices(option =>
{
    option.R_DisableAuthentication();
});

builder.Services.AddTransient<R_IReport, R_ReportService>();
builder.Services.AddSingleton<R_IEnvironment, BlazorMenuEnvironmentService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", builder.HostEnvironment.Environment);

var host = builder.Build();

host.R_SetupBlazorService();

await host.RunAsync();
