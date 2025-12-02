using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MomiaTrainSync.Core.Interfaces.Repositories.Base
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id, bool asNoTracking = true);

        Task<TEntity?> FirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

        Task<List<TEntity>> GetAllAsync(
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool includeInactive = false);

        Task<(List<TEntity> Data, int Total)> GetPagedAsync(
            int page,
            int pageSize,
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool includeInactive = false);

        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity?> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(int id);     // Hard delete
        Task<bool> SoftDeleteAsync(int id); // Soft delete si aplica
        Task<bool> RestoreSoftDeleteAsync(int id);
        Task<bool> ToggleEstadoAsync(int id);
    }
}
