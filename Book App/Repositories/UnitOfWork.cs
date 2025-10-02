using Book_App.Data;
using System.Threading.Tasks;

namespace Book_App.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository BookRepository { get; }
        IAuthorRepository AuthorRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task Save();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IBookRepository? bookRepository;
        private IAuthorRepository? authorRepository;
        private ICategoryRepository? categoryRepository;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBookRepository BookRepository => bookRepository ??= new BookRepository(_dbContext);
        public IAuthorRepository AuthorRepository => authorRepository ??= new AuthorRepository(_dbContext);
        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(_dbContext);
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {

                if (disposing)
                {
                    _dbContext.Dispose();
                }

            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
