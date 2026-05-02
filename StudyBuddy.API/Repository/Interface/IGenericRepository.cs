namespace StudyBuddy.API.Repository.Interface
{
    public interface IGenericRepository<T, TKey> where T : class
    {
    Task<int> Insert(T obj);
    Task<TKey> InsertReturnId(T obj);
    Task<int> InsertBulk(IEnumerable<T> obj);
    Task<bool> Update(T obj);
    Task<bool> Delete(object id);
    Task<T?> GetById(TKey id);
    Task<IEnumerable<T>> GetAll();
    Task<IEnumerable<T>> GetByColumn(string columnName, object value);

    }
}