using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Moq;
using VehicleStatusTracker;
using VehicleStatusTracker.Controllers;
using VehicleStatusTracker.DataModels;
using VehicleStatusTracker.Services;


namespace TestVehicles
{
    public class UnitTest1
    {
        private readonly ApplicationDbContext _context;
        private readonly Random _random;
        ILogger<VehicleService> _logger;


        //[Fact]
        //public void GetAllVehicles_Test()
        //{
        //    // Arrange
        //    var vehicleCount = 7;
        //    var vehicleServiceMock = new Mock<IVehicleService>();
        //    var loggerMock = new Mock<ILogger<VehicleController>>();
        //    var controller = new VehicleController(vehicleServiceMock.Object, loggerMock.Object);

        //    // Act
        //    var result = controller.Get();

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var vehiclesInResult = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(okResult.Value);
        //    Assert.Equal(vehicleCount, vehiclesInResult.ToList().Count);
        //}
        [Fact]
        public void Get_ReturnsOkResultWithListOfVehicles()
        {
            var config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json") // This should be the path to your appsettings.json in the test project
             .Build();

            // Arrange
            var vehicles = new List<Vehicle>

        {
            new Vehicle { Id = 1, VIN="YS2R4X20005399401",RegistrationNumber="ABC123",Status="Connected" },
            new Vehicle { Id = 2 , VIN="VLUR4X20009093588",RegistrationNumber="DEF456", Status="Connected"},
           
        };
             
        var loggerMock = new Mock<ILogger<VehicleController>>();

        var vehicleServiceMock = new Mock<IVehicleService>();
        vehicleServiceMock.Setup(service => service.GetAllVehicles()).Returns(vehicles);
        var idControllerMock = new Mock<IdController>(config);
        var controller = new VehicleController(vehicleServiceMock.Object, loggerMock.Object, idControllerMock.Object);

        // Act
        var result = controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var vehiclesInResult = Assert.IsAssignableFrom<IEnumerable<Vehicle>>(okResult.Value);

        Assert.Equal(vehicles.Count, vehiclesInResult.ToList().Count); // Check the count of vehicles
            Assert.Contains(vehicles[0], vehiclesInResult); // Check if a specific vehicle is in the result
            Assert.Contains(vehicles[1], vehiclesInResult); // Check if another specific vehicle is in the result
               
           
        }
        [Fact]
        public void GetAllVehicles_ByCustomer_Test()
        {
            // Arrange
            var customerId = 1;
            var customerName = "Kalles Grustransporter AB";
            var vehicleCount = 3;
            var context = new Mock<ApplicationDbContext>();
            var logger = new Mock<ILogger<VehicleService>>();
            var vehicleService = new VehicleService(context.Object, logger.Object);

            // Act
            var vehicles = vehicleService.GetVehiclesByCustomer(customerId);

            // Assert
            Assert.NotNull(vehicles);
            Assert.Equal(vehicleCount, vehicles.Count);
            Assert.Equal(customerId, vehicles[0].CustomerId);
            Assert.Equal(customerName, vehicles[0].Customer.Name);
        }
        [Fact]
        public void GetAllVehicles_ByStatus_Test()
        {
            // Arrange
            var status = "Connected";
            var context = new Mock<ApplicationDbContext>();
            var vehicleSet = new Mock<DbSet<Vehicle>>();

            // Set up DbSet to return an empty collection
            vehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.Provider).Returns(new List<Vehicle>().AsQueryable().Provider);
            vehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.Expression).Returns(new List<Vehicle>().AsQueryable().Expression);
            vehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.ElementType).Returns(new List<Vehicle>().AsQueryable().ElementType);
            vehicleSet.As<IQueryable<Vehicle>>().Setup(m => m.GetEnumerator()).Returns(new List<Vehicle>().AsQueryable().GetEnumerator());

            // Set up the context to return the DbSet
            context.Setup(c => c.Vehicles).Returns(vehicleSet.Object); var logger = new Mock<ILogger<VehicleService>>();
            var vehicleService = new VehicleService(context.Object, logger.Object);

            // Act
            var vehicles = vehicleService.GetVehiclesByStatus(status);

            // Assert
            Assert.NotNull(vehicles);
            Assert.Equal(status, vehicles[0].Status);
        }
    }

  
}
