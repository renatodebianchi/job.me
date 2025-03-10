using Domain.Interfaces.Repositories; 
using System.Threading.Tasks; 
using Domain.Interfaces.Services; 
using System.Linq; 
namespace Application.Services 
{ 
    public abstract class GenericService<TSource> : IGenericService<TSource> 
    { 
        protected IGenericRepository<TSource> Repository {get; private set;} 
        public GenericService(IGenericRepository<TSource> sourceRepository) 
        { 
            Repository = sourceRepository; 
        } 
        public async Task<TSource> GetByIdAsync(int id)  
        { 
            var result = await Repository.GetByIdAsync(id); 
            return result; 
        } 
        public async Task<IQueryable<TSource>> GetAllAsync() 
        { 
            var result = await Repository.GetAllAsync(); 
            return result; 
        } 
        public async Task<int> AddAsync(TSource entity)  
        { 
            return await Repository.AddAsync(entity); 
        } 
        public async Task<int> UpdateAsync(TSource entity) 
        { 
            return await Repository.UpdateAsync(entity); 
        } 
        public async Task<int> DeleteAsync(int id) 
        { 
            return await Repository.DeleteAsync(id); 
        } 
    } 
} 
