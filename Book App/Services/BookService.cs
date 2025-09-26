using Book_App.Data;
using Book_App.DTOs;
using Book_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAll();
        Task<Book> GetBookById(int id);
        Task<Category> GetCategoryById(int id);
        Task<Book> Add(CreateBookDTO bookDto);
        Task<Book> Update(int id,CreateBookDTO book);
        Task<bool> DeleteById(int id);
        Task<Category> AddCategory(Category category);
        Task<List<Category>> GetAllCategories();
        Task<bool> DeleteCategoryById (int id);
    }
    public class BookService : IBookService
    {
        private readonly AppDbContext _dbContext;
        public BookService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Book>> GetAll() => await _dbContext.Books.Include(b => b.Authors).Include(b=>b.Categories).ToListAsync();

        public async Task<Book> GetBookById(int id) => await _dbContext.Books.Include(b => b.Authors).Include(b => b.Categories).FirstOrDefaultAsync(a => a.Id ==id);

        public async Task<Category> GetCategoryById(int id) => await _dbContext.Categories.Include(b => b.Books).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<Category>> GetAllCategories() => await _dbContext.Categories.Include(c => c.Books).ToListAsync();

        public async Task<Book> Add(CreateBookDTO bookDto)
        {
            var authors = await _dbContext.Authors.Where(a => bookDto.AuthorIds.Contains(a.Id)).ToListAsync();
           
            var categories =await  _dbContext.Categories.Where(c=> bookDto.CategoryIds.Contains(c.Id)).ToListAsync();

            var newBook = new Book
            {
                Title = bookDto.Title,
                ISBN_10 = bookDto.ISBN_10,
                Price = bookDto.Price,
                Authors = authors,
                Categories = categories
            };

            _dbContext.Books.Add(newBook);
            await _dbContext.SaveChangesAsync();
            return newBook;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Book> Update(int id, CreateBookDTO book)
        {
            var existingBook =await  GetBookById(id);
            if (existingBook != null)
            {
                var authorIds = book.AuthorIds.ToList();
                var authors = await _dbContext.Authors.Where(a => authorIds.Contains(a.Id)).ToListAsync();
                var categoryIds = book.CategoryIds.ToList();
                var categories = await _dbContext.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();

                existingBook.Title = book.Title;
                existingBook.ISBN_10 = book.ISBN_10;
                existingBook.Price = book.Price;
                existingBook.Authors = authors;
                existingBook.Categories = categories;
                _dbContext.Books.Update(existingBook);
                await _dbContext.SaveChangesAsync();
                //await _dbContext.Entry(existingBook).Collection(b => b.Authors).LoadAsync();
                //await _dbContext.Entry(existingBook).Collection(b => b.Categories).LoadAsync();
            }
            
            return existingBook;
        }
        public async Task<bool> DeleteById(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public async Task<bool> DeleteCategoryById(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if(category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
