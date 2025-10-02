using AutoMapper;
using Book_App.Data;
using Book_App.DTOs;
using Book_App.Exceptions;
using Book_App.Models;
using Book_App.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Book_App.Services
{
    public interface IAuthorService
    {
        Task<AuthorDTO> GetById(int id);
        Task<AuthorInfoDTO> Add(CreateAuthorDTO author);
        Task<AuthorInfoDTO> Update(int id, CreateAuthorDTO author);
    }
    public class AuthorService: IAuthorService
    {
        private IUnitOfWork UOW;
        private readonly IMapper _mapper;
        public AuthorService(IUnitOfWork uow, IMapper mapper)
        {
            UOW = uow;
            _mapper = mapper;
        }
        public async Task<AuthorDTO> GetById(int id)
        {
            var author = await UOW.AuthorRepository.GetByIdWithBooks(id);
            return author==null? throw new NotFoundException("Author not found") : _mapper.Map<AuthorDTO>(author);
        }

        public async Task<AuthorInfoDTO> Add(CreateAuthorDTO authorDTO)
        {
            var author = new Author { Name=authorDTO.Name};
            await UOW.AuthorRepository.AddAsync(author);
            await UOW.Save();
            return _mapper.Map<AuthorInfoDTO>(author);
        }

        public async Task<AuthorInfoDTO> Update(int id, CreateAuthorDTO author)
        {
            var existingAuthor = await UOW.AuthorRepository.GetByIdAsync(id);
            if (existingAuthor != null)
            {
                existingAuthor.Name = author.Name;
                UOW.AuthorRepository.Update(existingAuthor);
                await UOW.Save();
            }
            
            return _mapper.Map<AuthorInfoDTO>(existingAuthor);
        }
    }
}
