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
        Task DeleteById(int id);
        Task<Category> AddCategory(Category category);
    }
    public class BookService : IBookService
    {
        private readonly AppDbContext _dbContext;
        public BookService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Book>> GetAll() => _dbContext.Books.Include(b => b.Authors).Include(b=>b.Categories).ToList();

        public async Task<Book> GetBookById(int id) => _dbContext.Books.Include(b => b.Authors).Include(b => b.Categories).FirstOrDefault(a => a.Id ==id);

        public async Task<Category> GetCategoryById(int id) => _dbContext.Categories.Include(b => b.Books).FirstOrDefault(a => a.Id == id);

        public async Task<Book> Add(CreateBookDTO bookDto)
        {
            var authors = _dbContext.Authors.Where(a => bookDto.AuthorIds.Contains(a.Id)).ToList();
           
            var categories = _dbContext.Categories.Where(c=> bookDto.CategoryIds.Contains(c.Id)).ToList();

            var newBook = new Book
            {
                Title = bookDto.Title,
                ISBN_10 = bookDto.ISBN_10,
                Price = bookDto.Price,
                Authors = authors,
                Categories = categories
            };

            _dbContext.Books.Add(newBook);
            _dbContext.SaveChanges();
            return newBook;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
            return category;
        }

        public async Task<Book> Update(int id, CreateBookDTO book)
        {
            var existingBook = GetBookById(id);
            if (existingBook == null)
            {
                return null;
            }
            var authorIds = book.AuthorIds.ToList();
            var authors = _dbContext.Authors.Where(a => authorIds.Contains(a.Id)).ToList();
            var categoryIds = book.CategoryIds.ToList();
            var categories = _dbContext.Categories.Where(c=> categoryIds.Contains(c.Id)).ToList();

            existingBook.Title = book.Title;
            existingBook.ISBN_10 = book.ISBN_10;
            existingBook.Price = book.Price;
            existingBook.Authors = authors;
            existingBook.Categories = categories;
            _dbContext.Books.Update(existingBook);
            _dbContext.SaveChanges();
            _dbContext.Entry(existingBook).Collection(b => b.Authors).Load();
            _dbContext.Entry(existingBook).Collection(b=>b.Categories).Load();
            return existingBook;
        }
        public Task DeleteById(int id)
        {
            var book = _dbContext.Books.Find(id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                _dbContext.SaveChanges();
            }

        }


    }
}
