using Cantina.Application.Interfaces;
using Cantina.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CantinaAPI.Infrastructure.Data;

namespace Cantina.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CantinaContext _context;
        private readonly ILogger<GenericRepository<T>> _logger;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CantinaContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _logger = logger;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all records of {Entity}", typeof(T).Name);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating {Entity}", typeof(T).Name);
                throw;
            }
        }

        public async Task<T> GetByIdAsync(object id, CancellationToken cancellationToken)
        {
            try
            {
                return await _dbSet.FindAsync(new[] { id }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching {Entity} with ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                await _dbSet.AddAsync(entity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding a new {Entity}", typeof(T).Name);
                throw;
            }
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating {Entity}", typeof(T).Name);
                throw;
            }
        }

        public async Task DeleteAsync(object id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await GetByIdAsync(id, cancellationToken);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting {Entity} with ID {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Set<T>()
                                     .Where(predicate)
                                     .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "$An error occurred while retrieving entity of type {EntityType} using predicate: {Predicate}", typeof(T).Name, predicate);
                throw;
            }
        }
    }
}
