using R_APIStartUp;

var builder = WebApplication.CreateBuilder(args);

builder.R_RegisterServices(option =>
{
    option.R_DisableAuthentication();
    option.R_DisableAuthorization();
    //option.R_DisableFastReport();
    //option.R_DisableOpenTelemetry();
    option.R_DisableReportServerClient();
    //option.R_DisableContext();
});

var app = builder.Build();

app.R_SetupMiddleware();

//app.UseSerilogIngestion();
//app.UseSerilogRequestLogging();
app.UseStaticFiles(); //blazor

app.Run();
