using Microsoft.EntityFrameworkCore;
using System;
using VehicleStatusTracker.Controllers;
using VehicleStatusTracker.DataModels;
using VehicleStatusTracker.Services;

namespace VehicleStatusTracker
{
    public class VehicleService : IVehicleService
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;
        ILogger<VehicleService> _logger;

        public VehicleService(ApplicationDbContext context, ILogger<VehicleService> logger)
        {
            _context = context;
            _random = new Random();
            _logger = logger;
        }
        public List<Vehicle> GetAllVehicles()
        {
            // Retrieve all vehicles from the database
            return _context.Vehicles.Include(v => v.Customer).ToList();
        }

        public List<Vehicle> GetVehiclesByCustomer(int customerId)
        {
            // Retrieve vehicles for a specific customer from the database
            return _context.Vehicles.Where(v => v.CustomerId == customerId).ToList();
        }

        public List<Vehicle> GetVehiclesByStatus(string status)
        {
            // Retrieve vehicles with a specific status from the database
            return _context.Vehicles.Where(v => v.Status == status).ToList();
        }
        // Simulate real-time status updates for vehicles
        public void SimulateRealTimeStatusUpdates()
        {
            try
            {
                var vehicles = _context.Vehicles.ToList();

                foreach (var vehicle in vehicles)
                {
                    // Simulate a status update (connected or disconnected)
                    vehicle.Status = _random.Next(0, 100) < 80 ? "Connected" : "Disconnected"; // 80% chance of being connected
                }

                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "{Method}: An error occurred while simulating vehicle status: {errorMessage}", nameof(SimulateRealTimeStatusUpdates), ex.Message);
            }

        }
    }

}

