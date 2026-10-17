namespace LibraryMS.BLL.DTOs.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<string> AuthorNames { get; set; } = new();
    }
}
