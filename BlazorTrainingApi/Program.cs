using R_APIStartUp;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.R_RegisterServices(option =>
{
    option.R_DisableAuthentication();
    option.R_DisableFastReport();
    option.R_DisableReportServerClient();
});

var app = builder.Build();

app.R_SetupMiddleware();

//app.UseSerilogIngestion();
app.UseSerilogRequestLogging();

app.Run();
