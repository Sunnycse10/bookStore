using Book_App.Data;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface IAuthorService
    {
        Author GetAuthorById(int id);
        Author AddAuthor(Author author);

    }
    public class AuthorService: IAuthorService
    {
        private readonly AppDbContext _dbContext;
        public AuthorService(AppDbContext context)
        {
            _dbContext = context;
        }
        public Author GetAuthorById(int id) => _dbContext.Authors.Include(a => a.Books).FirstOrDefault(a => a.Id == id);

        public Author AddAuthor(Author author)
        {
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
            return author;
        }
    }
}
