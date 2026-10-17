namespace LibraryMS.BLL.DTOs.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int CategoryId { get; set; }
        public List<int> AuthorIds { get; set; } = new();
    }
}
