namespace WebAPI_simple.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }

    public class AuthorNoIdDTO
    {
        public string FullName { get; set; }
    }
    public class AuthorWithBooksDTO
    {
        public string FullName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}