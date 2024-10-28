namespace Cronos.Scheduler.WebApi;

public static class AppMapSetupServer
{
    public static void AppMapSetup(this WebApplication app)
    {
        app.MapGet("/", () => "Cronos Service is running.");

        app.MapGet("/CronosGetStatus", 
            (
                CronosHostedService service) =>
                    {
                        return new CronosHostedServiceState(service.IsEnabled);
                    }
            );

        app.MapMethods("/CronosSetStatus", new[] { "PATCH" }, 
            (
                CronosHostedServiceState state,
                CronosHostedService service) =>
                    {
                        service.IsEnabled = state.IsEnabled;
                    }
            );

        app.MapMethods("/CronosSetMaxTask", new[] { "PATCH" },
            (
                CronosHostedServiceMaxTask maxTaskCount,
                CronosHostedService service) =>
            {
                service.MaxTasks = maxTaskCount.maxTask;
            }
            );

        app.MapPost("/CronosStop",
            (
                CronosHostedService service) =>
                    {
                        service.StopIt = true;
                    }
            );
    }
}
