namespace Cronos.Scheduler.Infra;

class ServiceRepositoryTwo
{
    public async Task DoServiceTwoAsync()
    {
        Log.Information("Service Two: running.");
        await Task.Delay(500);
    }

}
