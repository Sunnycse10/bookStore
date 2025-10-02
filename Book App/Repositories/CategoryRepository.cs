using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Repositories
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Category> GetByIdWithBooks(int id);
        Task<IEnumerable<Category>> GetAllWithBooks();
    }
    public class CategoryRepository:GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public CategoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbContext.Categories.Where(a=> ids.Contains(a.Id)).ToListAsync();
        }

        public async Task<Category> GetByIdWithBooks(int id)
        {
            return await _dbContext.Categories.Include(b => b.Books).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Category>> GetAllWithBooks()
        {
            return await _dbContext.Categories.Include(c=>c.Books).ToListAsync();
        }


    }
}
