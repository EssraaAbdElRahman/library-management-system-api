using LibraryMS.DAL.Repositories.Implementation;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryMS.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;
        private IDbContextTransaction? _transaction;

        private IBookRepository? _books;
        private IAuthorRepository? _authors;
        private ICategoryRepository? _categories;
        private IMemberRepository? _members;
        private IBorrowRecordRepository? _borrowRecords;
        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        }
        public IBookRepository Books => _books ??= new BookRepository(_context);
        public IAuthorRepository Authors => _authors ??= new AuthorRepository(_context);
        public ICategoryRepository Categories => _categories ??= new CategoryRepository(_context);
        public IMemberRepository Members => _members ??= new MemberRepository(_context);
        public IBorrowRecordRepository BorrowRecords => _borrowRecords ??= new BorrowRecordRepository(_context);

        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException("A transaction is already active.");
            
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
