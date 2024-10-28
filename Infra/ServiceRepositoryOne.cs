namespace Cronos.Scheduler.Infra;

class ServiceRepositoryOne
{
    private readonly ILogger<ServiceRepositoryOne> _logger;

    public ServiceRepositoryOne(ILogger<ServiceRepositoryOne> logger)
    {
        _logger = logger;
    }

    public async Task DoServiceOneAsync()
    {
        _logger.LogInformation("Service One : running.");
        await Task.Delay(1000);
    }

}
