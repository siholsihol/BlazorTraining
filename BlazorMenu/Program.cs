using BlazorMenu.Extensions;
using BlazorMenu.Services;
using BlazorPrettyCode;
using BlazorTraining.Controls.Preload;
using DataNorthwindHttpProvider.Extensions;

//using DataNorthwindHttpProvider.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorFrontEnd.FileConverter;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_BlazorFrontEnd.Report;
using R_BlazorFrontEnd.Tenant.Extensions;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BlazorMenu.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices();

builder.Services.R_AddBlazorMenuServices();

builder.Services.AddTransient<R_IFileConverter, R_FileConverter>();
builder.Services.AddTransient<R_IReport, R_ReportService>();
builder.Services.AddSingleton<R_IFileDownloader, R_FileDownloader>();
builder.Services.AddTransient<HttpInterceptorService>();
builder.Services.AddSingleton<R_IEnvironment, BlazorMenuEnvironmentService>();
builder.Services.AddScoped<R_PreloadMenuService>();

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", builder.HostEnvironment.Environment);

builder.Services.AddMultiTenantancy();

builder.Services.AddBlazorPrettyCode();

//builder.Services.AddDataDummyProvider(); //with bogus
builder.Services.AddDataHttpProvider(); //with http file
//builder.Services.AddSingleton<ICategoryService, DataNorthwindSqlProvider.CategoryService>();

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//builder.Services.AddHttpClient(DataNorthwindSqlProvider.Constants.HttpClientConstant.Name, client =>
// {
//     client.DefaultRequestHeaders.AcceptLanguage.Clear();
//     client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
//     client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//     client.BaseAddress = new Uri("http://localhost:5108");
// });

var host = builder.Build();

host.Services.AddServiceProviderToTenantRoutes();

host.R_SetupBlazorService();

await host.R_UseBlazorMenuServices();

await host.RunAsync();
