using System; 
using System.Threading.Tasks; 
using Domain.Interfaces.Entities; 
using Microsoft.EntityFrameworkCore; 
using System.Linq; 
using Domain.Extensions; 
using Infra.Data.Contexts;
using Domain.Interfaces.Repositories;
namespace Infra.Data.Repositories.EntityFramework 
{ 
    public class BaseRepositoryEntityFramework<TSource> : IGenericRepository<TSource> where TSource : class, IGenericEntity 
    { 
        protected readonly ApplicationDbContext _context; 
        public BaseRepositoryEntityFramework(ApplicationDbContext context) 
        { 
            _context = context; 
        } 
        public virtual async Task<int> AddAsync(TSource entity) 
        { 
            try { 
                entity.CreatedAt = DateTime.UtcNow.ToLocalTime(); 
                await _context.AddAsync(entity); 
                await _context.SaveChangesAsync(); 
                return entity.Id; 
            } catch (Exception e) { 
                Console.WriteLine($"Insert record error: {e.Message}"); 
                throw; 
            } 
        } 
        public virtual async Task<int> DeleteAsync(int id) 
        { 
            try { 
                var entity = await this.GetByIdAsync(id); 
                _context.Remove(entity); 
                await _context.SaveChangesAsync(); 
                return 1; 
            } catch (Exception e) { 
                Console.WriteLine($"Delete record error: {e.Message}"); 
                throw; 
            } 
        } 
        public virtual async Task<IQueryable<TSource>> GetAllAsync() 
        { 
            await Task.Delay(1); 
            return _context.Set<TSource>(); 
        } 
        public virtual async Task<TSource> GetByIdAsync(int id) 
        { 
            return await _context.Set<TSource>().Where(obj => obj.Id == id).FirstOrDefaultAsync(); 
        } 
        public virtual async Task<int> UpdateAsync(TSource entity) 
        { 
            return await this.UpdateAsync(entity, false); 
        } 
        public virtual async Task<int> UpdateAsync(TSource entity, bool force) 
        { 
            try { 
                var updEntity = await this.GetByIdAsync(entity.Id); 
               if (updEntity == null) 
                   throw new InvalidDataException($"No record found for Id: {entity.Id}"); 
               if (!force) 
                   updEntity.SpreadNotNull(entity); 
               else 
                   updEntity.Spread(entity); 
               updEntity.UpdatedAt = DateTime.UtcNow.ToLocalTime(); 
               _context.Update(updEntity); 
               await _context.SaveChangesAsync(); 
               return updEntity.Id;  
            } catch (Exception e) { 
                Console.WriteLine($"Update record error: {e.Message}"); 
                throw; 
            } 
        } 
    } 
} 
