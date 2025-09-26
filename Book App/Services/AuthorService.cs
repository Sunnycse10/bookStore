using Book_App.Data;
using Book_App.DTOs;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface IAuthorService
    {
        Task<Author> GetAuthorById(int id);
        Task<Author> AddAuthor(Author author);
        Task<Author> UpdateAuthor(int id, CreateAuthorDTO author);
    }
    public class AuthorService: IAuthorService
    {
        private readonly AppDbContext _dbContext;
        public AuthorService(AppDbContext context)
        {
            _dbContext = context;
        }
        public async Task<Author> GetAuthorById(int id) =>await  _dbContext.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<Author> AddAuthor(Author author)
        {
            _dbContext.Authors.Add(author);
            await _dbContext.SaveChangesAsync();
            return author;
        }

        public async Task<Author> UpdateAuthor(int id, CreateAuthorDTO author)
        {
            var existingAuthor = await GetAuthorById(id);
            if (existingAuthor != null)
            {
                existingAuthor.Name = author.Name;
                _dbContext.Authors.Update(existingAuthor);
                await _dbContext.SaveChangesAsync();
            }
            
            return existingAuthor;
        }
    }
}
