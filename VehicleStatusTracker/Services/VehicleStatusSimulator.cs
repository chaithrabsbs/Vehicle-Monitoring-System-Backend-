using System;
using System.Threading;


namespace VehicleStatusTracker.Services
{
    public class VehicleStatusSimulator : IHostedService, IDisposable
    {
        //private readonly IVehicleService _vehicleService;//can not use scoped services within singleton services, due to lifetime issues.
        private readonly IServiceProvider _serviceProvider;
        private Timer _statusUpdateTimer;
        private readonly int statusUpdateInterval = 60000; // 60 seconds (1 minute)

        public VehicleStatusSimulator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _statusUpdateTimer = new Timer(UpdateVehicleStatus, null, 0, statusUpdateInterval);
        }

        private void UpdateVehicleStatus(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();
                vehicleService.SimulateRealTimeStatusUpdates();
                Console.WriteLine("Vehicle status updated.");
            }
            
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _statusUpdateTimer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _statusUpdateTimer?.Dispose();
        }
    }
}
