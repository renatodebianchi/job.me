using Domain.Interfaces.Entities; 
using Domain.Interfaces.Repositories; 
namespace Infra.Data.Repositories 
{ 
    public abstract class BaseRepository<TSource> : IGenericRepository<TSource> where TSource : IGenericEntity 
    { 
        public abstract Task<int> AddAsync(TSource entity); 
        public abstract Task<int> DeleteAsync(int id); 
        public abstract Task<IQueryable<TSource>> GetAllAsync(); 
        public abstract Task<TSource> GetByIdAsync(int id); 
        public abstract Task<int> UpdateAsync(TSource entity); 
        public abstract Task<int> UpdateAsync(TSource entity, bool force); 
        
    } 
} 
