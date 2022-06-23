using WebApplication2.Models.Book;

namespace WebApplication2.Models.Author
{
    public class AuthorDetailsDTO : AuthorReadOnlyDTO
    {
        public List<BookReadOnlyDTO> Books { get; set; }
    }
}
