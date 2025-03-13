using MediatR; 
namespace Application.Requests { 
    public abstract class BaseRequest<T> : IRequest<T>
    { 
        public BaseRequest() 
        { 
            Notifications = new Dictionary<string, string>(); 
        } 
        public Dictionary<string, string> Notifications; 
        public abstract bool IsValid(); 
    } 
} 
