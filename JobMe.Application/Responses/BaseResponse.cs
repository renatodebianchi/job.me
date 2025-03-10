namespace Application.Responses { 
    public class BaseResponse 
    { 
        public bool IsSuccessful { get; set; } = false;
        public string? Message { get; set; }
        public object? Data { get; set; }
    } 
} 
