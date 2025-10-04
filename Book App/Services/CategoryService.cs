using AutoMapper;
using Book_App.DTOs;
using Book_App.Exceptions;
using Book_App.Models;
using Book_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface ICategoryService
    {
        Task<CategoryWithBooksDTO> GetById(int id);
        Task<CategoryDTO> Add(CreateCategoryDTO category);
        Task<List<CategoryWithBooksDTO>> GetAll();
        Task<bool> Delete(int id);
    }
    public class CategoryService:ICategoryService
    {
        private IUnitOfWork UOW;
        private readonly IMapper _mapper;
        public CategoryService(IUnitOfWork uow,IMapper mapper)
        {
            UOW = uow;
            _mapper = mapper;
        }
        public async Task<CategoryWithBooksDTO> GetById(int id)
        {

            var category = await UOW.CategoryRepository.GetByIdWithBooks(id);
            return category == null ? throw new NotFoundException("Category not found") : _mapper.Map<CategoryWithBooksDTO>(category);
        }

        public async Task<List<CategoryWithBooksDTO>> GetAll()
        {
            var categories = await UOW.CategoryRepository.GetAllWithBooks();
            return _mapper.Map<List<CategoryWithBooksDTO>>(categories);
        }
        public async Task<CategoryDTO> Add(CreateCategoryDTO createCategoryDTO)
        {
            var category = new Category { Title = createCategoryDTO.Title };
            await UOW.CategoryRepository.AddAsync(category);
            await UOW.Save();
            return _mapper.Map<CategoryDTO>(category);
        }

        public async Task<bool> Delete(int id)
        {
            var category = await UOW.CategoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                UOW.CategoryRepository.Delete(category);
                await UOW.Save();
                return true;
            }
            return false;
        }
    }
}
