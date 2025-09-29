using AutoMapper;
using Book_App.Models;

namespace Book_App.DTOs
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Book,BookDTO>()
                .ForMember(dest=>dest.Categories,opt=>opt.MapFrom(src=>src.Categories))
                .ForMember(dest=>dest.Authors, opt=>opt.MapFrom(src=>src.Authors));
            CreateMap<Author, AuthorInfoDTO>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<BookDTO, Book>()
                .ForMember(dest=>dest.Categories, opt=>opt.Ignore())
                .ForMember(dest=>dest.Authors, opt=>opt.Ignore());
            CreateMap<Category, CategoryWithBooksDTO>()
                .ForMember(dest=>dest.Books, opt=>opt.MapFrom(src=>src.Books));
            CreateMap<Author, AuthorDTO>()
                .ForMember(dest=>dest.Books, opt=>opt.MapFrom(src=>src.Books));
            CreateMap<Book, BookInfoDTO>();
        }
    }
}
