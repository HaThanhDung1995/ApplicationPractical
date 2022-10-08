namespace ApplicationPractical.Controllers
{
    public class ApiResponse<TData>
    {
        public TData Data { get; set; }
        public string MessageDetail { get; set; }
        public string Status { get; set; }
        public string MessageCode { get; set; }
    }
}
