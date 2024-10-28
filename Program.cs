WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ServiceRepositoryOne>();
builder.Services.AddScoped<ServiceRepositoryTwo>();

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
