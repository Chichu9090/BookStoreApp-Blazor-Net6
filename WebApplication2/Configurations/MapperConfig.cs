using AutoMapper;
using WebApplication2.Data;
using WebApplication2.Models.Author;
using WebApplication2.Models.Book;
using WebApplication2.Models.User;

namespace WebApplication2.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();  
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();  
            CreateMap<AuthorReadOnlyDTO, Author>().ReverseMap();  

            CreateMap<BookCreateDTO, Author>().ReverseMap();  
            CreateMap<BookUpdateDTO, Author>().ReverseMap();  
            CreateMap<Book, BookReadOnlyDTO>()
                .ForMember(connString => connString.AuthorName,d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();
            CreateMap<Book, BookDetailsDTO>()
                .ForMember(connString => connString.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

            CreateMap<APIUser, UserDto>().ReverseMap();
        }
    }
}
