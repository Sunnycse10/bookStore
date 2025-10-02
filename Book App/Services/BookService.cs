using AutoMapper;
using Book_App.Data;
using Book_App.DTOs;
using Book_App.Exceptions;
using Book_App.Models;
using Book_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface IBookService
    {
        Task<List<BookDTO>> GetAll();
        Task<BookDTO> GetById(int id);
        Task<BookDTO> Add(CreateBookDTO bookDto);
        Task<BookDTO> Update(int id,CreateBookDTO book);
        Task<bool> Delete(int id);
    }
    public class BookService : IBookService
    {
        private IUnitOfWork UOW;
        private readonly IMapper _mapper;
        public BookService(IUnitOfWork uow, IMapper mapper)
        {
            UOW = uow;
            _mapper = mapper;
        }

        public async Task<List<BookDTO>> GetAll()
        {
            var books =  await UOW.BookRepository.GetAllBooksWithAuthorsAndCategories();
            return _mapper.Map<List<BookDTO>>(books);
        }

        public async Task<BookDTO> GetById(int id)
        {
            var book = await UOW.BookRepository.GetByIdWithAuthorsAndCategories(id);
            return _mapper.Map<BookDTO>(book);
        }

       

        public async Task<BookDTO> Add(CreateBookDTO bookDto)
        {
            if(bookDto.AuthorIds.Count == 0) throw new NotFoundException("Authors not found!");
            var authors = (await UOW.AuthorRepository.GetByIdsAsync(bookDto.AuthorIds)).ToList();
            if (!authors.Any()) throw new NotFoundException("Authors not found!");
            if (authors.Count != bookDto.AuthorIds.Count)
            {
                var missingIds = bookDto.AuthorIds.Except(authors.Select(a => a.Id));
                throw new NotFoundException($"Authors not found: {string.Join(", ", missingIds)}");
            }

            if (bookDto.CategoryIds.Count == 0) throw new NotFoundException("Categories not found!");
            var categories = (await UOW.CategoryRepository.GetByIdsAsync(bookDto.CategoryIds)).ToList();
            if(!categories.Any()) throw new NotFoundException("Categories not found!");          

            if (categories.Count != bookDto.CategoryIds.Count)
            {
                var missingIds = bookDto.CategoryIds.Except(categories.Select(c => c.Id));
                throw new NotFoundException($"Categories not found: {string.Join(", ", missingIds)}");
            }

            var newBook = new Book
            {
                Title = bookDto.Title,
                ISBN_10 = bookDto.ISBN_10,
                Price = bookDto.Price,
                Authors = authors,
                Categories = categories
            };

            await UOW.BookRepository.AddAsync(newBook);
            await UOW.Save();
            return _mapper.Map<BookDTO>(newBook);
        }


        public async Task<BookDTO?> Update(int id, CreateBookDTO book)
        {
            var existingBook = await  UOW.BookRepository.GetByIdWithAuthorsAndCategories(id);
            if (existingBook != null)
            {
                var authorIds = book.AuthorIds.ToList();
                var authors = (await UOW.AuthorRepository.GetByIdsAsync(authorIds)).ToList();
                var categoryIds = book.CategoryIds.ToList();
                var categories = (await UOW.CategoryRepository.GetByIdsAsync(categoryIds)).ToList();

                existingBook.Title = book.Title;
                existingBook.ISBN_10 = book.ISBN_10;
                existingBook.Price = book.Price;
                existingBook.Authors = authors;
                existingBook.Categories = categories;
                UOW.BookRepository.Update(existingBook);
                await UOW.Save();
            }
            
            return (existingBook!=null)? _mapper.Map<BookDTO>(existingBook): null;
        }
        public async Task<bool> Delete(int id)
        {
            var book = await UOW.BookRepository.GetByIdAsync(id);
            if (book != null)
            {
                UOW.BookRepository.Delete(book);
                await UOW.Save();
                return true;
            }
            return false;

        }

       
    }
}
