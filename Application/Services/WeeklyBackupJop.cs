 using Microsoft.Extensions.Hosting;
namespace WebAPI.Application.Services
{
   

    public class WeeklyBackupJop : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WeeklyBackupJop(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var backupService = scope.ServiceProvider.GetRequiredService<DatabaseBackupService>();

                await backupService.RunBackupIfNeededAsync();

                
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
   
}
