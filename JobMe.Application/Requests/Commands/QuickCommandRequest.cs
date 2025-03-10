namespace Application.Requests.Commands { 
    public abstract class QuickCommandRequest<Model> : QuickRequest
    { 
        public Model? DataModel {get; set;} 
        public QuickCommandRequest(): base() 
        {} 
    } 
} 
