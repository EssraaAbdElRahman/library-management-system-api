namespace LibraryMS.DAL.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public string? CreatedBy { get; private set; }

        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }

        public DateTime? DeletedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public string? DeletedBy { get; private set; }

        protected BaseEntity()
        {
        }
        public void Created(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedAt = DateTime.Now;
        }
        public void Updated(string updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedAt = DateTime.Now;
        }

        public void Deleted(string deletedBy)
        {
            if (IsDeleted)
                throw new InvalidOperationException("Entity is already deleted.");

            if (string.IsNullOrWhiteSpace(deletedBy))
                throw new ArgumentException("Deleted by is required.");
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeletedAt = DateTime.Now;
        }
        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }
    }
}
