using System.Linq.Expressions;

namespace Imdb.Persistence.Repository.Interfaces;

public interface IRepository<T>
{
        Task<T> CreateItem(T item);
        Task<T> UpdateItem(T item);
        IQueryable<T> FindItems();
        Task<T> FindSingleItem(Expression<Func<T, bool>> match);
        IQueryable<T> FindItems(Expression<Func<T, bool>> match);
        Task<T> FindItem(long id);
        Task<int> SaveChanges();
}
