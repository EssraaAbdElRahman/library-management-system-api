namespace LibraryMS.BLL.DTOs.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public int BooksCount { get; set; }
    }
}
