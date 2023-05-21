using R_APIStartUp;

var builder = WebApplication.CreateBuilder(args);

builder.R_RegisterServices(option =>
{
    //option.R_DisableAuthentication();
});

var app = builder.Build();

app.R_SetupMiddleware();

app.Run();
