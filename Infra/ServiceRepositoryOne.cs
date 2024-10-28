namespace Cronos.Scheduler.Infra;

class ServiceRepositoryOne
{
    public async Task DoServiceOneAsync()
    {
        Log.Information("Service One : running.");
        await Task.Delay(1000);
    }

}
