using System;

namespace Persistence.Data;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task<T> Add(T entity);
    Task<T?> Update(T entity);
    Task<T?> Update(int id, Action<T> updateAction);
    Task<T?> Delete(int id);
}
