namespace VehicleStatusTracker.Models
{
    public class UserModel
    {
        public string LoginID { get; set; }
        public string Password { get; set; }
        public string UserMessage { get; set; } = string.Empty;
        public string UserToken { get; set; } = string.Empty;
    }
}
