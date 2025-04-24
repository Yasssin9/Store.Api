using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenrericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        private readonly StoreDbContext _storeDbContext;

        public GenericRepository(StoreDbContext storeDbContext)
        {
            _storeDbContext = storeDbContext;
        }
        public async Task AddAsync(TEntity entity)
        => await _storeDbContext.AddAsync(entity);

        public async Task<int> CountAsync(Specification<TEntity> specification)        
           => await ApplySpecification(specification).CountAsync();


        private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
            => SpecificationEvaluator.GetQuery(_storeDbContext.Set<TEntity>(), specification);

        public void Delete(TEntity entity)
        =>  _storeDbContext.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity?>> GetAllAsync(bool isTrackable = false)
        {
            if (isTrackable)
               return await _storeDbContext.Set<TEntity>().ToListAsync();
            return await _storeDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity?>> GetAllAsync(Specification<TEntity> specification)
        {
            return await SpecificationEvaluator.GetQuery(_storeDbContext.Set<TEntity>(), specification).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Tkey id)
        => await _storeDbContext.Set<TEntity>().FindAsync(id);

        public async Task<TEntity?> GetAsync(Specification<TEntity> specification)
        {
            return await SpecificationEvaluator.GetQuery(_storeDbContext.Set<TEntity>(), specification).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        => _storeDbContext.Set<TEntity>().Update(entity);
    }
}
