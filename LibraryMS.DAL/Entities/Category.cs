namespace LibraryMS.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;

        // Navigation Property
        public ICollection<Book> Books { get; private set; } = new List<Book>();
        private Category()
        {

        }
        public Category(string name,string createdBy)
        {

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name of category is required");
            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("User is required");
            Name = name.Trim();
            Created(createdBy);
        }
        public void UpdateCategory(string name ,string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name of category is required");
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required");
            Name = name.Trim();
            Updated(updatedBy);
        }
    }
}
