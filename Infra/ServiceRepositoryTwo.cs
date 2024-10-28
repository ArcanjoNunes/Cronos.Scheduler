namespace Cronos.Scheduler.Infra;

class ServiceRepositoryTwo
{
    private readonly ILogger<ServiceRepositoryTwo> _logger;

    public ServiceRepositoryTwo(ILogger<ServiceRepositoryTwo> logger)
    {
        _logger = logger;
    }

    public async Task DoServiceTwoAsync()
    {
        _logger.LogInformation("Service Two: running.");
        await Task.Delay(500);
    }

}
