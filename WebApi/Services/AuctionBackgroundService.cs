using Domain.Interfaces.Services;

namespace WebApi.Services
{
    public class AuctionBackgroundService : BackgroundService, IAuctionBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuctionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PeriodicTimer timer = new(TimeSpan.FromMilliseconds(30000));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var service = scope.ServiceProvider.GetService<IAuctionEndingService>();
                    try
                    {
                        //await Console.Out.WriteLineAsync("1231534346464");
                        await service.ProceedEndedAuctionAsync();
                    }
                    catch (Exception ex) { await Console.Out.WriteLineAsync(ex.Message); }

                    
                }

                var memory = GC.GetTotalMemory(true);
                //await Console.Out.WriteLineAsync(memory.ToString());
                if (memory >= 200000000) GC.Collect();
            }
        }
    }
}
