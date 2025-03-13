using System.ComponentModel.DataAnnotations; 
using Domain.Interfaces.Entities; 
namespace Domain.Entities 
{ 
    public abstract class GenericEntity : IGenericEntity 
    {
        public GenericEntity() 
        { 
            Id = 0;
            CreatedAt = DateTime.Now; 
        }

        [Key] 
        public int Id {get; set;} 
        public DateTime CreatedAt {get; set;} 
        public DateTime? UpdatedAt {get; set;} 
    } 
} 
