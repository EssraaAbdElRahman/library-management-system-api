using LibraryMS.BLL.DTOs.Category;

namespace LibraryMS.BLL.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdWithBooksAsync(id);
            if (category is null)
                throw new NotFoundException($"Category with id {id} not found.");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto, string createdBy)
        {
            if (await _unitOfWork.Categories.IsNameExistsAsync(dto.Name))
                throw new BusinessRuleException($"A category with name '{dto.Name}' already exists.");

            var category = new Category(dto.Name, createdBy);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto, string updatedBy)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category is null)
                throw new NotFoundException($"Category with id {id} not found.");

            if (await _unitOfWork.Categories.IsNameExistsAsync(dto.Name, excludeCategoryId: id))
                throw new BusinessRuleException($"A category with name '{dto.Name}' already exists.");

            category.UpdateCategory(dto.Name, updatedBy);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task DeleteAsync(int id, string deletedBy)
        {
            var category = await _unitOfWork.Categories.GetByIdWithBooksAsync(id);
            if (category is null)
                throw new NotFoundException($"Category with id {id} not found.");

            if (category.Books.Any())
                throw new BusinessRuleException("Cannot delete a category linked to existing books.");

            category.Deleted(deletedBy);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}