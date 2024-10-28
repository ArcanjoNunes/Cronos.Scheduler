namespace Cronos.Scheduler.Infra;

class CronosHostedService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(5);
    private readonly IServiceScopeFactory _factory;
    private int _executionCount = 0;
    
    public bool IsEnabled { get; set; }
    public bool StopIt { get; set; } = false;
    public int MaxTasks { get; set; } = 1;

    public CronosHostedService(IServiceScopeFactory factory)
    {
        _factory = factory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // ExecuteAsync is executed once and we have to take care of a mechanism ourselves that is kept during operation.
        // To do this, we can use a Periodic Timer, which, unlike other timers, does not block resources.
        // But instead, WaitForNextTickAsync provides a mechanism that blocks a task and can thus be used in a While loop.
        
        using PeriodicTimer timer = new PeriodicTimer(_period);

        // When ASP.NET Core is intentionally shut down, the background service receives information
        // via the stopping token that it has been canceled.
        // We check the cancellation to avoid blocking the application shutdown.

        while (!stoppingToken.IsCancellationRequested && 
                await timer.WaitForNextTickAsync(stoppingToken))
        {
            if (StopIt) 
            { 
                Log.Information($"* USER REQUEST: STOPPING CRONOS.");
                break; 
            }
            
            try
            {
                if (IsEnabled)
                {
                    // We cannot use the default dependency injection behavior, because ExecuteAsync is
                    // a long-running method while the background service is running.
                    // To prevent open resources and instances, only create the services and other references on a run

                    // Create scope, so we get request services

                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();

                    // Get services from scope

                    Log.Information($" *** Running Service One");
                    ServiceRepositoryOne serviceRepositoryOne = asyncScope.ServiceProvider.GetRequiredService<ServiceRepositoryOne>();
                    await serviceRepositoryOne.DoServiceOneAsync();

                    Log.Information($" *** Running Service Two");
                    ServiceRepositoryTwo serviceRepositoryTwo = asyncScope.ServiceProvider.GetRequiredService<ServiceRepositoryTwo>();
                    await serviceRepositoryTwo.DoServiceTwoAsync();

                    Log.Information($" *** Running Service Three");
                    ServiceRepositoryThree serviceRepositoryThree = asyncScope.ServiceProvider.GetRequiredService<ServiceRepositoryThree>();
                    await serviceRepositoryThree.DoServiceThreeAsync();

                    _executionCount++;
                    Log.Information($"Executed PeriodicHostedService - Count: {_executionCount}");

                    if (_executionCount >= MaxTasks)
                    {
                        Log.Information($"* MAX TASKS REACHED. STOPPING CRONOS.");
                        break;
                    }

                }
                else
                {
                    Log.Information("Skipped PeriodicHostedService");
                }
            }
            catch (Exception ex)
            {
                Log.Information($"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
            }
        }
    }
}
