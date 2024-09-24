using System.Linq.Expressions;
using Imdb.Persistence.DatabaseContext;
using Imdb.Persistence.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Imdb.Persistence.Repository.Implementation;

public class Repository<T> : IRepository<T>
    where T : class {
        private readonly MovieDbContext _dbContext;
        private readonly ILogger<Repository<T>> _logger;
        public Repository (MovieDbContext dbContext, ILogger<Repository<T>> logger) {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<T> CreateItem (T item) {
            var result = await _dbContext.Set<T> ().AddAsync (item);
            await _dbContext.SaveChangesAsync ();
            return result.Entity;
        }

        public async Task<T> FindItem (long id) {
            return await _dbContext.Set<T> ().FindAsync (id);
        }

        public IQueryable<T> FindItems () {
            return _dbContext.Set<T> ().AsNoTracking ();
        }

        public IQueryable<T> FindItems (Expression<Func<T, bool>> match) {
            return _dbContext.Set<T> ().AsNoTracking ().Where (match);
        }

        public async Task<T> FindSingleItem (Expression<Func<T, bool>> match) {
            return await _dbContext.Set<T> ().AsNoTracking ().FirstOrDefaultAsync (match);
        }

        public async Task<int> SaveChanges () {
            return await _dbContext.SaveChangesAsync ();
        }

        public async Task<T?> UpdateItem (T item) {
            var result = _dbContext.Set<T> ().Update (item);
            var status = await _dbContext.SaveChangesAsync ();
            return status > 0 ? result.Entity : null;
        }
    }