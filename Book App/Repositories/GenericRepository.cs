using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Book_App.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
    public class GenericRepository<T> : IGenericRepository<T> where T: class
    {
        private readonly AppDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet= _dbContext.Set<T>();

        }

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity); 

        public void Delete(T entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAllAsync()=>await _dbSet.ToListAsync();

        public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);

        public void Update(T entity)=> _dbSet.Update(entity);
    }
}
