using VehicleStatusTracker.DataModels;

namespace VehicleStatusTracker.Services
{
    public interface IVehicleService
    {
        List<Vehicle> GetAllVehicles();
        List<Vehicle> GetVehiclesByCustomer(int customerId);
        List<Vehicle> GetVehiclesByStatus(string status);
        void SimulateRealTimeStatusUpdates();
    }
}
