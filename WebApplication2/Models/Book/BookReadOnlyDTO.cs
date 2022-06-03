namespace WebApplication2.Models.Book
{
    public class BookReadOnlyDTO : BaseDTO
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public string AuthorName { get; set; }
    }
}
