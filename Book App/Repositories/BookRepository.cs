using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Repositories
{
    public interface IBookRepository: IGenericRepository<Book>
    {
        Task<Book> GetByIdWithAuthorsAndCategories(int id);
        Task<IEnumerable<Book>> GetAllBooksWithAuthorsAndCategories();
    }
    public class BookRepository:GenericRepository<Book>, IBookRepository
    {
        private readonly AppDbContext _dbContext;

        public BookRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithAuthorsAndCategories()
        {
           return await _dbContext.Books.Include(b => b.Authors).Include(b => b.Categories).ToListAsync();
        }

        public async Task<Book> GetByIdWithAuthorsAndCategories(int id)
        {
            return await _dbContext.Books.Include(b => b.Authors).Include(b => b.Categories).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
