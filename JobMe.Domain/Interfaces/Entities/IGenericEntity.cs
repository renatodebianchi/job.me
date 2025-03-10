using System; 
namespace Domain.Interfaces.Entities 
{ 
    public interface IGenericEntity 
    { 
        int Id {get; set;} 
        DateTime CreatedAt {get; set;} 
        DateTime? UpdatedAt {get; set;} 
    } 
} 
