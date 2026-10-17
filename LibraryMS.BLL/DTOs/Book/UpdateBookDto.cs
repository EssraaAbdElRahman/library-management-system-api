namespace LibraryMS.BLL.DTOs.Book
{
    public class UpdateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int CategoryId { get; set; }
    }
}
