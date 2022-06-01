namespace WebApplication2.Models.Author
{
    public class AuthorReadOnlyDTO : BaseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Bio { get; set; }
    }
}
