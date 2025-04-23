using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenrericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        Task<TEntity?> GetAsync(Tkey id);
        Task<TEntity?> GetAsync(Specification<TEntity> specification);
        Task<IEnumerable<TEntity?>> GetAllAsync(bool isTrackable=false);
        Task<IEnumerable<TEntity?>> GetAllAsync(Specification<TEntity> specification);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
