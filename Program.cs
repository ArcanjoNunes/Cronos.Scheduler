WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Serilog 

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() //.File("LogRegister/Cronos-Log.txt", rollingInterval: RollingInterval.Day) // Create folder first
    .CreateLogger();

builder.Services.AddSerilog();

// User's Repositories (Cronos Events)

builder.Services.AddScoped<ServiceRepositoryOne>();
builder.Services.AddScoped<ServiceRepositoryTwo>();
builder.Services.AddScoped<ServiceRepositoryThree>();

// Register as singleton first so it can be injected through Dependency Injection

builder.Services.AddSingleton<CronosHostedService>();

// Add as hosted service using the instance registered as singleton before

builder.Services.AddHostedService
    (
        provider => provider.GetRequiredService<CronosHostedService>()
    );


WebApplication app = builder.Build();

AppMapSetupServer.AppMapSetup(app);

app.Run();
