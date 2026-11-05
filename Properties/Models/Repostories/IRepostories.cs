namespace ECommerceApi.Repostories;

public interface IRepostories<T> where T : class
{
    List<T> GetAll();
    T? GetById(int id);
    IQueryable<T> Query();
    void Add(T entity);
    void Delete(T entity);
    void SaveChanges();
}