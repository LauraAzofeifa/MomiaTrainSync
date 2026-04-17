using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MomiaTrainSync.Core.Interfaces.Repositories.Base;
using MomiaTrainSync.Core.Interfaces.Repositories.Logging;
using MomiaTrainSync.Infrastructure.Persistence;
using MomiaTrainSync.Domain.Common;

namespace MomiaTrainSync.Infrastructure.Repositories.Base
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly MomiaTrainSyncDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ILogErrorRepository _logger;

        public GenericRepository(
            MomiaTrainSyncDbContext context,
            ILogErrorRepository logger)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _logger = logger;
        }

        protected async Task Log(string method, Exception ex)
        {
            await _logger.AddLogAsync($"{typeof(TEntity).Name}.{method}", ex);
        }

        // ============================================================
        // GET BY ID (con soporte include + tracking / no-tracking)
        // ============================================================
        public virtual async Task<TEntity?> GetByIdAsync(
            int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool asNoTracking = true)
        {
            try
            {
                // Obtener nombre de la PK de la entidad
                var key = _context.Model.FindEntityType(typeof(TEntity))!
                    .FindPrimaryKey()!
                    .Properties
                    .First()
                    .Name;

                IQueryable<TEntity> query = _dbSet;

                // Apply Include (si aplica)
                if (include != null)
                    query = include(query);

                // Tracking / No Tracking
                if (asNoTracking)
                    query = query.AsNoTracking();

                // Buscar por ID
                return await query
                    .FirstOrDefaultAsync(e => EF.Property<int>(e, key) == id);
            }
            catch (Exception ex)
            {
                await Log(nameof(GetByIdAsync), ex);
                return null;
            }
        }


        // ============================================================
        // FIRST/WHERE
        // ============================================================
        public virtual async Task<TEntity?> FirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                if (include != null)
                    query = include(query);

                if (asNoTracking)
                    query = query.AsNoTracking();

                return await query.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                await Log(nameof(FirstAsync), ex);
                return null;
            }
        }

        // ============================================================
        // GET ALL
        // ============================================================
        public virtual async Task<List<TEntity>> GetAllAsync(
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool includeInactive = false)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                if (asNoTracking)
                    query = query.AsNoTracking();

                if (include != null)
                    query = include(query);

                // SoftDelete general
                if (!includeInactive && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                {
                    query = query.Where(e => EF.Property<bool>(e, "Estado") == true);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                await Log(nameof(GetAllAsync), ex);
                return new List<TEntity>();
            }
        }

        // ============================================================
        // COUNT
        // ============================================================
        public virtual async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            bool includeInactive = false)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;
                if (predicate != null)
                    query = query.Where(predicate);
                if (!includeInactive && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                {
                    query = query.Where(e => EF.Property<bool>(e, "Estado") == true);
                }
                return await query.CountAsync();
            }
            catch (Exception ex)
            {
                await Log(nameof(CountAsync), ex);
                return 0;
            }
        }

        // ============================================================
        // PAGED (para tablas grandes, datatables, APIs...)
        // ============================================================
        public virtual async Task<(List<TEntity> Data, int Total)> GetPagedAsync(
            int page,
            int pageSize,
            bool asNoTracking = true,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            bool includeInactive = false)
        {
            try
            {
                IQueryable<TEntity> query = _dbSet;

                if (include != null)
                    query = include(query);

                if (!includeInactive && typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                {
                    query = query.Where(e => (e as ISoftDelete)!.Estado);
                }

                if (asNoTracking)
                    query = query.AsNoTracking();

                var total = await query.CountAsync();
                var data = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return (data, total);
            }
            catch (Exception ex)
            {
                await Log(nameof(GetPagedAsync), ex);
                return (new List<TEntity>(), 0);
            }
        }

        // ============================================================
        // ADD
        // ============================================================
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await Log(nameof(AddAsync), ex);
                throw;
            }
        }

        // ============================================================
        // UPDATE
        // ============================================================
        public virtual async Task<TEntity?> UpdateAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                var saved = await _context.SaveChangesAsync() > 0;

                if (!saved)
                    return null;

                return entity; // ← AQUÍ
            }
            catch (Exception ex)
            {
                await Log(nameof(UpdateAsync), ex);
                throw;
            }
        }

        // ============================================================
        // HARD DELETE
        // ============================================================
        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id, asNoTracking:false);
                if (entity == null)
                    return false;

                _dbSet.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await Log(nameof(DeleteAsync), ex);
                throw;
            }
        }

        // ============================================================
        // SOFT DELETE (solo si la entidad lo soporta)
        // ============================================================
        public virtual async Task<bool> SoftDeleteAsync(int id)
        {
            try
            {
                if (!typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                {
                    return await DeleteAsync(id);
                }

                var entity = await GetByIdAsync(id, asNoTracking: false);
                if (entity == null)
                    return false;

                // Cambiar Estado = false
                typeof(TEntity).GetProperty("Estado")!.SetValue(entity, false);

                _dbSet.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await Log(nameof(SoftDeleteAsync), ex);
                throw;
            }
        }

        public virtual async Task<bool> RestoreSoftDeleteAsync(int id)
        {
            try
            {
                if (!typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                    return false;

                var entity = await GetByIdAsync(id, asNoTracking: false);
                if (entity == null) return false;

                typeof(TEntity).GetProperty("Estado")!.SetValue(entity, true);

                _dbSet.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await Log(nameof(RestoreSoftDeleteAsync), ex);
                throw;
            }
        }

        public virtual async Task<bool> ToggleEstadoAsync(int id)
        {
            try
            {
                if (!typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
                    return false;

                var entity = await GetByIdAsync(id, asNoTracking: false);
                if (entity == null) return false;

                var prop = typeof(TEntity).GetProperty("Estado")!;
                bool current = (bool)prop.GetValue(entity)!;

                prop.SetValue(entity, !current);

                _dbSet.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                await Log(nameof(ToggleEstadoAsync), ex);
                throw;
            }
        }


    }
}