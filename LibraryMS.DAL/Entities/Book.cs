namespace LibraryMS.DAL.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string ISBN { get; private set; } = string.Empty;
        public int PublishedYear { get; private set; }
        public int TotalCopies { get; private set; }
        public int AvailableCopies { get; private set; }

        // Foreign Key
        public int CategoryId { get; private set; }
        public Category? Category { get; private set; }

        // Navigation Properties
        public ICollection<Author> Authors { get; private set; } = new List<Author>();
        public ICollection<BorrowRecord> BorrowRecords { get; private set; } = new List<BorrowRecord>();

        private Book()
        {

        }

        public Book(string title, string iSBN, int publishedYear, int totalCopies
            , int categoryId, string createdBy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title of book is required");

            if (string.IsNullOrWhiteSpace(iSBN))
                throw new ArgumentException("Name of book is required");

            if (string.IsNullOrWhiteSpace(createdBy))
                throw new ArgumentException("User  is required");
            if (publishedYear <= 0)
                throw new ArgumentException("Published year must be greater than zero.");

            if (totalCopies <= 0)
                throw new ArgumentException("Total copies must be greater than zero.");

            if (categoryId <= 0)
                throw new ArgumentException("Category is required.");
            Title = title.Trim();
            ISBN = iSBN.Trim();
            PublishedYear = publishedYear;
            TotalCopies = totalCopies;
            AvailableCopies = totalCopies;
            CategoryId = categoryId;
            Created(createdBy.Trim());
        }
        public void UpdateBook(string title, string iSBN, int publishedYear
            , int categoryId, string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title of category is required");
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User of category is required");

            if (string.IsNullOrWhiteSpace(iSBN))
                throw new ArgumentException("Name of category is required");

            if (publishedYear <= 0)
                throw new ArgumentException("Published year must be greater than zero.");


            if (categoryId <= 0)
                throw new ArgumentException("Category is required.");
            Title = title.Trim();
            ISBN = iSBN.Trim();
            PublishedYear = publishedYear;
            CategoryId = categoryId;
            Updated(updatedBy.Trim());
        }
        public void AddCopies(int count, string updatedBy)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.");

            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            TotalCopies += count;
            AvailableCopies += count;

            Updated(updatedBy.Trim());
        }
        public void RemoveCopies(int count, string updatedBy)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than zero.");

            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            if (count > AvailableCopies)
                throw new InvalidOperationException(
                    "Cannot remove more copies than available copies.");

            TotalCopies -= count;
            AvailableCopies -= count;

            Updated(updatedBy.Trim());
        }
        public void BorrowCopy(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            if (AvailableCopies <= 0)
                throw new InvalidOperationException(
                    "The book is not available for borrowing.");

            AvailableCopies--;

            Updated(updatedBy.Trim());
        }

        public void ReturnCopy(string updatedBy)
        {
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");

            if (AvailableCopies >= TotalCopies)
                throw new InvalidOperationException(
                    "All copies are already available.");

            AvailableCopies++;

            Updated(updatedBy.Trim());
        }
        public void AddAuthor(Author author, string updatedBy)
        {
            if (author is null)
                throw new ArgumentException("Author is required.");
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");
            if (Authors.Any(a => a.Id == author.Id))
                throw new InvalidOperationException("This author is already linked to the book.");

            Authors.Add(author);
            Updated(updatedBy.Trim());
        }
        public void RemoveAuthor(int authorId, string updatedBy)
        {
            var author = Authors.FirstOrDefault(a => a.Id == authorId);
            if (author is null)
                throw new InvalidOperationException("This author is not linked to the book.");
            if (string.IsNullOrWhiteSpace(updatedBy))
                throw new ArgumentException("User is required.");
            Authors.Remove(author);
            Updated(updatedBy);
        }
    }
}