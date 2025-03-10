namespace Application.Requests.Commands { 
    public abstract class BaseCommandRequest<T, Model> : BaseRequest<T> 
    { 
        public Model? DataModel {get; set;} 
        public BaseCommandRequest(): base() 
        {} 
    } 
} 
