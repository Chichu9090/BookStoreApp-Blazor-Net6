using AutoMapper;
using WebApplication2.Data;
using WebApplication2.Models.Author;

namespace WebApplication2.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDTO, Author>().ReverseMap();  
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();  
            CreateMap<AuthorReadOnlyDTO, Author>().ReverseMap();  
        }
    }
}
