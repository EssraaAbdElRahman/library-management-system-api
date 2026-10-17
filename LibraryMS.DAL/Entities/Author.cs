namespace LibraryMS.DAL.Entities
{
    public class Author: BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string? Bio { get; private set; }

        public ICollection<Book> Books { get; private set; } = new List<Book>();
        private Author()
        {
        }
        public Author(string name, string? bio,string createdBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name of author is required");
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("User is of author is required");

            Name = name.Trim();
            Bio = bio?.Trim() ;
            Created(createdBy.Trim());
        }
        public void UpdateAuthor(string name, string? bio,string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("name of author is required");

            if(string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is of author is required");

            Name = name.Trim();
            Bio = bio?.Trim();
            Updated(updatedBy.Trim());
        }
    }
}
