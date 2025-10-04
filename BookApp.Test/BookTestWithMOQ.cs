using AutoMapper;
using Book_App.DTOs;
using Book_App.Repositories;
using Book_App.Services;
using Book_App.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Book_App.Exceptions;

namespace BookApp.Test
{
    public class BookTestWithMOQ
    {
        //Since the services access the repository through UnitOfWork, Create a private field for mock UnitOfWork interface
        private readonly Mock<IUnitOfWork> _mockUOW;

        //Create a private field for mock Repository
        //private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        //private readonly Mock<IAuthorRepository> _mockAuthorRepo;
       
        //Creare private field for Services that need to be tested
        private readonly IBookService _bookService;
        private readonly ILogger<BookTestWithMOQ> _logger;
        private readonly IMapper _mapper;
        public BookTestWithMOQ()
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            _logger = loggerFactory.CreateLogger<BookTestWithMOQ>();
            var expression = new MapperConfigurationExpression();
            expression.AddMaps(typeof(MappingProfile).Assembly);
            var config = new MapperConfiguration(expression, loggerFactory);

            _mapper = config.CreateMapper();

            //Create a mock instance for IUnitOfWork interface
            _mockUOW = new Mock<IUnitOfWork>();
            // _mockUOW.Object is a mock IUnitOfWork object
            _bookService = new BookService(_mockUOW.Object, _mapper);

        }


        [Fact]
        public async Task Create_Book_With_Valid_Input()
        {
            //Arrange
            var Ids = new List<int> { 1, 2 };
            var authors = new List<Author> { new Author { Id = 1, Name = "Person 1" }, new Author { Id = 2, Name = "Person 2" } };
            var categories = new List<Category> { new Category { Id = 1, Title = "Category 1" }, new Category { Id = 2, Title = "Category 2" } };
            //Setup the GetByIdsAsync method of AuthorRepository to return a valid response when being called
            _mockUOW.Setup(r => r.AuthorRepository.GetByIdsAsync(Ids)).ReturnsAsync(authors);

            //Setup the GetByIdsAsync method of CategoryRepository to return a valid response when being called
            _mockUOW.Setup(r => r.CategoryRepository.GetByIdsAsync(Ids)).ReturnsAsync(categories);

            //Setup the BookRepository mock's AddAsync Method to do nothing (Complete the task) but be marked as Verifiable
            //So whenever AddAsync is called with any Book object, just return a completed Task
            //In verify, it checks "Is this method called with any Book?"
            _mockUOW.Setup(r=>r.BookRepository.AddAsync(It.IsAny<Book>())).Returns(Task.CompletedTask).Verifiable();
            _mockUOW.Setup(r=>r.Save()).Returns(Task.CompletedTask);

           
            var bookDTO = new CreateBookDTO { Title = "Demo Book", ISBN_10 = "123456", Price = 100, AuthorIds = Ids, CategoryIds = Ids };
            //Act
            var result = await _bookService.Add(bookDTO);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(bookDTO.Title, result.Title);
            _mockUOW.Verify(r => r.BookRepository.AddAsync(It.IsAny<Book>()), Times.Once);

        }

        //Unit test for Author not found
        [Fact]
        public async Task Create_Book_Authors_Not_Found()
        {
            //Arrange
            var Ids = new List<int> { 1, 2 };
            var authors = new List<Author> { new Author { Id = 1, Name = "Person 1" }, new Author { Id = 2, Name = "Person 2" } };
            var categories = new List<Category> { new Category { Id=1, Title="Category 1" }, new Category { Id=2, Title="Category" } };

            var testIds= new List<int> { 1,2,3 };
            _mockUOW.Setup(r=>r.AuthorRepository.GetByIdsAsync(testIds)).ReturnsAsync(authors);

            _mockUOW.Setup(r=>r.CategoryRepository.GetByIdsAsync(testIds)).ReturnsAsync(categories);

            var bookDTO = new CreateBookDTO { Title ="Demo Book", ISBN_10="12324324", Price=50, AuthorIds=testIds, CategoryIds=testIds };

            var result = await Assert.ThrowsAsync<NotFoundException>(()=> _bookService.Add(bookDTO));
            Assert.Equal("Authors not found: 3", result.Message);

        }
        //Unit test for nonExistent Book Id
        [Fact]
        public async Task Get_Book_With_NonExistent_Id()
        {
            var testId = 100;
            _mockUOW.Setup(r => r.BookRepository.GetByIdWithAuthorsAndCategories(testId)).ReturnsAsync((Book?)null);

            var result = await _bookService.GetById(testId);

            Assert.Null(result);
            _mockUOW.Verify(r=>r.BookRepository.GetByIdWithAuthorsAndCategories(testId), Times.Once());
        }
    }
}
