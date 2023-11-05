using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleStatusTracker.Controllers;
using VehicleStatusTracker.DataModels;
using VehicleStatusTracker.Models;
using VehicleStatusTracker.Services;
using Xunit;
using FluentAssertions;
using Newtonsoft.Json;

public class VehicleControllerTests
{
    private readonly string testDataFile = "TestData.json";

    [Fact]
    public void Get_ReturnsListOfVehicles()
    {
        // Arrange
        var vehicleServiceMock = new Mock<IVehicleService>();
        var loggerMock = new Mock<ILogger<VehicleController>>();
        var controller = new VehicleController(vehicleServiceMock.Object, loggerMock.Object);

    // Read the JSON file content
    string json = System.IO.File.ReadAllText(testDataFile);

        // Deserialize the JSON into a List<Vehicle>
        List<Vehicle> testVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);

        // Act
        vehicleServiceMock.Setup(service => service.GetAllVehicles()).Returns(testVehicles); // Mocking the behavior of IVehicleService
        var result = controller.Get();

        // Assert
        // Assuming result is an ActionResult<IEnumerable<Vehicle>>
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Vehicle>>>(result);
        // Extract and convert the data from the ActionResult to a List of Vehicle objects
        var vehicles = ((actionResult.Result as ObjectResult)?.Value as IEnumerable<Vehicle>).ToList();
        vehicles.Should().HaveCount(testVehicles.Count);
    }

    [Fact]
    public void GetByCustomer_ReturnsListOfVehicles()
    {
        // Arrange
        var vehicleServiceMock = new Mock<IVehicleService>();
        var loggerMock = new Mock<ILogger<VehicleController>>();
        var controller = new VehicleController(vehicleServiceMock.Object, loggerMock.Object);
        int customerId = 2;

        // Read the JSON file content
        string json = System.IO.File.ReadAllText(testDataFile);

        // Deserialize the JSON into a List<Vehicle>
        List<Vehicle> testVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);

        // Act
        // Setup the mock service to return a specific list of vehicles for a given customer ID
        vehicleServiceMock.Setup(service => service.GetVehiclesByCustomer(It.IsAny<int>()))
     .Returns((int customerId) =>
     {
         // Filter the list of vehicles based on the customer ID
         var filteredVehicles = testVehicles.Where(vehicle => vehicle.CustomerId == customerId).ToList();
         return filteredVehicles;
     });
        var result = controller.GetByCustomer(customerId);

        // Assuming result is an ActionResult<IEnumerable<Vehicle>>
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Vehicle>>>(result);
       
        // Extract and convert the data from the ActionResult to a List of Vehicle objects
        var vehicles = ((actionResult.Result as ObjectResult)?.Value as IEnumerable<Vehicle>).ToList();

        // Assert that all vehicles have the CustomerId == 2
        Assert.True(vehicles.All(vehicle => vehicle.CustomerId == 2));
    }

    [Fact]
    public void GetByStatus_ReturnsListOfVehicles()
    {
        // Arrange
        var vehicleServiceMock = new Mock<IVehicleService>();
        var loggerMock = new Mock<ILogger<VehicleController>>();
        var controller = new VehicleController(vehicleServiceMock.Object, loggerMock.Object);
        string status = "Connected";

        // Read the JSON file content
        string json = System.IO.File.ReadAllText(testDataFile);

        // Deserialize the JSON into a List<Vehicle>
        List<Vehicle> testVehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);

        // Act
        // Setup the mock service to return a specific list of vehicles for a given status
        vehicleServiceMock.Setup(service => service.GetVehiclesByStatus(It.IsAny<string>()))
            .Returns((string status) =>
            {
                // Filter the list of vehicles based on the status
                var filteredVehicles = testVehicles.Where(vehicle => vehicle.Status == status).ToList();
                return filteredVehicles;
            }); var result = controller.GetByStatus(status);

        // Assert
        // Assuming result is an ActionResult<IEnumerable<Vehicle>>
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Vehicle>>>(result);
        
        // Extract and convert the data from the ActionResult to a List of Vehicle objects
        var vehicles = ((actionResult.Result as ObjectResult)?.Value as IEnumerable<Vehicle>).ToList();
        
        // Assert that all vehicles have the status "Connected"
        Assert.True(vehicles.All(vehicle => vehicle.Status == status));
    }
}
