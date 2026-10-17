using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.BLL.Service.Implementation
{
    public class BookService:IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdWithDetailsAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            return _mapper.Map<BookDto>(book);
        }

        public async Task<List<BookDto>> SearchAsync(string? title, int? categoryId, int page, int pageSize)
        {
            var books = await _unitOfWork.Books.SearchAsync(title, categoryId, page, pageSize);
            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto, string createdBy)
        {
            // 1. تأكد إن الـ Category موجودة
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category is null)
                throw new NotFoundException($"Category with id {dto.CategoryId} not found.");

            // 2. تأكد إن الـ ISBN مش مكرر
            if (await _unitOfWork.Books.IsIsbnExistsAsync(dto.ISBN))
                throw new BusinessRuleException($"A book with ISBN {dto.ISBN} already exists.");

            // 3. اعمل الـ Book (يدوي، مش AutoMapper)
            var book = new Book(dto.Title, dto.ISBN, dto.PublishedYear, dto.TotalCopies, dto.CategoryId, createdBy);

            // 4. اربط المؤلفين
            foreach (var authorId in dto.AuthorIds)
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(authorId);
                if (author is null)
                    throw new NotFoundException($"Author with id {authorId} not found.");

                book.AddAuthor(author, createdBy);
            }

            // 5. أضف واحفظ
            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateAsync(int id, UpdateBookDto dto, string updatedBy)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            var category = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryId);
            if (category is null)
                throw new NotFoundException($"Category with id {dto.CategoryId} not found.");

            // لو غيّر الـ ISBN، تأكد إنه مش مكرر عند كتاب تاني
            if (await _unitOfWork.Books.IsIsbnExistsAsync(dto.ISBN, excludeBookId: id))
                throw new BusinessRuleException($"A book with ISBN {dto.ISBN} already exists.");

            book.UpdateBook(dto.Title, dto.ISBN, dto.PublishedYear, dto.CategoryId, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task DeleteAsync(int id, string deletedBy)
        {
            await _unitOfWork.Books.DeleteAsync(id, deletedBy);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<BookDto> AddCopiesAsync(int id, AddBookCopiesDto dto, string updatedBy)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            book.AddCopies(dto.Count, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> RemoveCopiesAsync(int id, RemoveBookCopiesDto dto, string updatedBy)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            book.RemoveCopies(dto.Count, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> AddAuthorAsync(int id, AddAuthorToBookDto dto, string updatedBy)
        {
            var book = await _unitOfWork.Books.GetByIdWithDetailsAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            var author = await _unitOfWork.Authors.GetByIdAsync(dto.AuthorId);
            if (author is null)
                throw new NotFoundException($"Author with id {dto.AuthorId} not found.");

            book.AddAuthor(author, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> RemoveAuthorAsync(int id, int authorId, string updatedBy)
        {
            var book = await _unitOfWork.Books.GetByIdWithDetailsAsync(id);
            if (book is null)
                throw new NotFoundException($"Book with id {id} not found.");

            book.RemoveAuthor(authorId, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<BookDto>(book);
        }
    }
}