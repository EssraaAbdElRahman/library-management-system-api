namespace LibraryMS.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        ICategoryRepository Categories { get; }
        IMemberRepository Members { get; }
        IBorrowRecordRepository BorrowRecords { get; }

        Task<int> SaveChangeAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
