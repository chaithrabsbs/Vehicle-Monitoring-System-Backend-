using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleStatusTracker.DataModels
{
    
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    
    public class Vehicle
    {
        public int Id { get; set; }
        public string VIN { get; set; }
        public string RegistrationNumber { get; set; }
        public string Status { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
