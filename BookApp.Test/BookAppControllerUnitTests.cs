using AutoMapper;
using Book_App.Controllers;
using Book_App.Data;
using Book_App.DTOs;
using Book_App.Models;
using Book_App.Repositories;
using Book_App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.Test
{
    public class BookAppControllerUnitTests
    {
        private readonly ILogger<BookAppControllerUnitTests> _logger;
        private readonly IMapper _mapper;
        //private readonly IUnitOfWork _unitOfWork;

        public BookAppControllerUnitTests()
        {
           
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                        
            _logger = loggerFactory.CreateLogger<BookAppControllerUnitTests>();
            var expression = new MapperConfigurationExpression();
            expression.AddMaps(typeof(MappingProfile).Assembly);            
            var config = new MapperConfiguration(expression, loggerFactory);
           
            _mapper = config.CreateMapper();
        }

        private async Task<AppDbContext> AddDemoData(AppDbContext dbContext)
        {
            var author = new Author { Name = "Thomas H. Cormen" };
            dbContext.Authors.Add(author);
            await dbContext.SaveChangesAsync();
            var category = new Category { Title = "Algorithms" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var book = new Book {
                Title = "Introduction to Algorithms",
                Authors = new  List<Author> { author} ,
                Categories = new List<Category> {category },
                Price = (decimal)25.50,
                ISBN_10= "0262531968"
            };
            dbContext.Books.Add(book);
            await dbContext.SaveChangesAsync();
            return dbContext;
         }


        private AppDbContext GetInMemoryDbContext()
        {
            var dboptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "BookAppTestDB_" + System.Guid.NewGuid()
                .ToString()).ConfigureWarnings(cfg => cfg.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            var dbContext = new AppDbContext(dboptions);
            return dbContext;
        }

        [Fact(Skip ="skip for now")]
        public async Task Create_Author_Test_With_Valid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var uow = new UnitOfWork(dbContext);
            var authorDTO = new CreateAuthorDTO { Name = "Thomas H. Cormen" };            
            var bookService = new BookService(uow,_mapper);
            var authorService = new AuthorService(uow, _mapper);
            var authorController = new authorsController(authorService);
            var result = await authorController.Create(authorDTO);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedAuthor = Assert.IsType<AuthorInfoDTO>(actionResult.Value);
            Assert.Equal(authorDTO.Name, returnedAuthor.Name);

        }

        [Fact(Skip ="Skip for now")]
        public async Task Get_Author_by_Id_With_Valid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var dbContextWithDemoData = await AddDemoData(dbContext);
            var uow = new UnitOfWork(dbContextWithDemoData);
            var authorService = new AuthorService(uow,_mapper);
            var author = await authorService.GetById(1);            
            var authorController = new authorsController(authorService);
            var result = await authorController.GetById(1);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedAuthor = Assert.IsType<AuthorDTO>(okObjectResult.Value);
            Assert.Equivalent(author, returnedAuthor);
        }

        [Fact(Skip ="Skip for now")]
        public async Task Get_Author_By_Non_Existent_ID()
        {
            var dbContext = GetInMemoryDbContext();
            var dbContextWithDemoData = await AddDemoData(dbContext);
            var uow = new UnitOfWork(dbContextWithDemoData);
            var authorService = new AuthorService(uow,_mapper);
            var authorController = new authorsController(authorService);
            var result = await authorController.GetById(100);
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact(Skip ="Skip for now")]
        public async Task Create_Author_With_Invalid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var uow = new UnitOfWork(dbContext);
            var inValidAuthorDto= new CreateAuthorDTO {Name=null};
            var authorService = new AuthorService(uow,_mapper);
            var authorController = new authorsController(authorService);
            authorController.ModelState.AddModelError("Name", "Required");
            var result = await authorController.Create(inValidAuthorDto);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>( badRequest.Value);
        }

        [Fact(Skip = "skip for now")]
        public async Task Create_Category_With_Valid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var uow = new UnitOfWork(dbContext);
            var categoryService = new CategoryService(uow,_mapper);
            var categoryDTO = new CreateCategoryDTO { Title = "Programming" };
            var categoryController = new categoriesController(categoryService);
            var result = await categoryController.Create(categoryDTO);
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCategory = Assert.IsType<CategoryDTO>(actionResult.Value);
            Assert.Equal(categoryDTO.Title, returnedCategory.Title);

        }
        [Fact]
        public async Task Get_Category_With_Valid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var dbContextWithDemoData = await AddDemoData(dbContext);
            var uow = new UnitOfWork(dbContextWithDemoData);
            var categoryService = new CategoryService(uow, _mapper);
            var category = await categoryService.GetById(1);
            var categoryDTO = _mapper.Map<CategoryWithBooksDTO>(category);
            var categoryController = new categoriesController(categoryService);
            var result = await categoryController.GetById(1);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsType<CategoryWithBooksDTO>(okObjectResult.Value);
            Assert.Equivalent(categoryDTO, returnedCategory);
        }
        [Fact]
        public async Task Create_Book_With_Valid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var dbContextWithDemoData = await AddDemoData(dbContext);
            var uow = new UnitOfWork(dbContextWithDemoData);
            var bookService = new BookService(uow,_mapper);
            var book = await bookService.GetById(1);
            var bookDTO = _mapper.Map<BookDTO>(book);
            var bookController = new booksController(bookService);
            var result = await bookController.GetById(1);
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBook = Assert.IsType<BookDTO>(okObjectResult.Value);
            Assert.Equivalent(bookDTO, returnedBook);
        }
        [Fact]
        public async Task Create_Book_With_Invalid_Input()
        {
            var dbContext = GetInMemoryDbContext();
            var uow = new UnitOfWork(dbContext);
            var author = new Author { Name = "Herbert Schildt" };
            dbContext.Authors.Add(author);
            await dbContext.SaveChangesAsync();
            var category = new Category { Title = "Programming" };
            dbContext.Categories.Add(category);
            await dbContext.SaveChangesAsync();
            var bookService = new BookService(uow, _mapper);    
            var bookController = new booksController(bookService);
            var createBookDTO = new CreateBookDTO { Title = "Teach Yourself C", Price = -200, ISBN_10 = "0078823110", AuthorIds = new List<int> { 1 }, CategoryIds = new List<int> { 1 } };
            var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await bookController.Create(createBookDTO));
            Assert.Equal("Price cannot be negative", ex.Message);
        }
    }
}
