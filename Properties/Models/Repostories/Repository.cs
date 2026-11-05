using ECommerceApi.Repostories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories;

public class Repository<T> : IRepostories<T> where T : class{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public List<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    
public void Delete(T entity)
{
    _dbSet.Remove(entity);
}


    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}