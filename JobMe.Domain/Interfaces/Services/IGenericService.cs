using System.Linq; 
using System.Threading.Tasks; 
using Domain.Interfaces.Repositories; 
namespace Domain.Interfaces.Services 
{ 
    public interface IGenericService<T> 
    { 
        Task<T> GetByIdAsync(int id); 
        Task<IQueryable<T>> GetAllAsync(); 
        Task<int> AddAsync(T entity); 
        Task<int> UpdateAsync(T entity); 
        Task<int> DeleteAsync(int id); 
    } 
} 
