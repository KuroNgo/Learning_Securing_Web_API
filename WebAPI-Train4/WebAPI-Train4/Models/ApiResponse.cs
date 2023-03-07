namespace WebAPI_Train4.Models
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } // Kieu truu tuong
    }
}
