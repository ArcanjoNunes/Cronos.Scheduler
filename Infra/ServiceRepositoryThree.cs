namespace Cronos.Scheduler.Infra;

class ServiceRepositoryThree
{
    public async Task DoServiceThreeAsync()
    {
        Log.Information("Service Three: running.");
        await Task.Delay(500);
    }

}
