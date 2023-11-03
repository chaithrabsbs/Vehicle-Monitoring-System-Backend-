using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleStatusTracker.DataModels;
using VehicleStatusTracker.Models;
using VehicleStatusTracker.Services;

namespace VehicleStatusTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly ILogger<VehicleController> _logger; // Inject the logger
        private readonly IdController _idController;
        public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger, IdController idController)
        {
            _vehicleService = vehicleService;
            _logger = logger;
            _idController = idController;
        }

        [Authorize]
        [HttpGet]
            public ActionResult<IEnumerable<Vehicle>> Get()
            {
            try
            {
                    var vehicles = _vehicleService.GetAllVehicles();
                    return Ok(vehicles);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method}: An error occurred while getting vehicles: {errorMessage}", nameof(Get), ex.Message);
                return StatusCode(500, "Internal Server Error");
            }
            
            }
        [Authorize]
        [HttpGet("customer/{customerId}")]
            public ActionResult<IEnumerable<Vehicle>> GetByCustomer(int customerId)
            {
            try
            {
                var vehicles = _vehicleService.GetVehiclesByCustomer(customerId);
                if (vehicles == null || vehicles.Count == 0)
                {
                    return NotFound($"No vehicles found for customer with ID {customerId}");
                }
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method}: An error occurred while getting vehicles: {errorMessage}", nameof(GetByCustomer), ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

        }
        [Authorize]
        [HttpGet("status/{status}")]
            public ActionResult<IEnumerable<Vehicle>> GetByStatus(string status)
            {
            try
            {
                var vehicles = _vehicleService.GetVehiclesByStatus(status);
                if (vehicles == null || vehicles.Count == 0)
                {
                    return NotFound($"No vehicles found with status: {status}");
                }
                return Ok(vehicles);
            }
                
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Method}: An error occurred while getting vehicles: {errorMessage}", nameof(GetByStatus), ex.Message);
                return StatusCode(500, "Internal Server Error");
            }

        }
    }
    }


