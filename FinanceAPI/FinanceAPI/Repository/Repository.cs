using FinanceAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinanceAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync(predicate);
        }

        // 🔥 New methods
        public async Task<IEnumerable<T>> FindWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _context.Set<T>();
            query = include(query);
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<T?> FirstOrDefaultWithIncludesAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _context.Set<T>();
            query = include(query);
            return await query.FirstOrDefaultAsync(predicate);
        }


    }

}
