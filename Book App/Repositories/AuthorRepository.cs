using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Repositories
{
    public interface IAuthorRepository:IGenericRepository<Author>
    {
        Task<Author> GetByIdWithBooks(int id);
        Task<IEnumerable<Author>> GetByIdsAsync(IEnumerable<int> ids);
    }
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly AppDbContext _dbContext;
        public AuthorRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _dbContext.Authors.Where(a => ids.Contains(a.Id)).ToListAsync();
        }

        public async Task<Author> GetByIdWithBooks(int id)
        {
            return await _dbContext.Authors.Include(a => a.Books).FirstOrDefaultAsync(a=> a.Id == id);
        }

    }
}
