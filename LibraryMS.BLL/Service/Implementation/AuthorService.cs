using LibraryMS.BLL.DTOs.Author;
namespace LibraryMS.BLL.Service.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            var author = await _unitOfWork.Authors.GetByIdWithBooksAsync(id);
            if (author is null)
                throw new NotFoundException($"Author with id {id} not found.");

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<List<AuthorDto>> GetAllAsync()
        {
            var authors = await _unitOfWork.Authors.GetAllAsync();
            return _mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<List<AuthorDto>> SearchByNameAsync(string name)
        {
            var authors = await _unitOfWork.Authors.SearchByNameAsync(name);
            return _mapper.Map<List<AuthorDto>>(authors);
        }

        public async Task<AuthorDto> CreateAsync(CreateAuthorDto dto, string createdBy)
        {
            var author = new Author(dto.Name, dto.Bio, createdBy);

            await _unitOfWork.Authors.AddAsync(author);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> UpdateAsync(int id, UpdateAuthorDto dto, string updatedBy)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id);
            if (author is null)
                throw new NotFoundException($"Author with id {id} not found.");

            author.UpdateAuthor(dto.Name, dto.Bio, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task DeleteAsync(int id, string deletedBy)
        {
            var author = await _unitOfWork.Authors.GetByIdWithBooksAsync(id);
            if (author is null)
                throw new NotFoundException($"Author with id {id} not found.");

            if (author.Books.Any())
                throw new BusinessRuleException("Cannot delete an author linked to existing books.");

            author.Deleted(deletedBy);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
