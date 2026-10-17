namespace LibraryMS.BLL.DTOs.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BooksCount { get; set; }
    }
}
