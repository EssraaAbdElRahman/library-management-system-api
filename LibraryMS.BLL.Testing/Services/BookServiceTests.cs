using AutoMapper;
using FluentAssertions;
using LibraryMS.BLL.DTOs.Book;
using LibraryMS.BLL.Service.Implementation;
using LibraryMS.DAL.Entities;
using LibraryMS.DAL.Exceptions;
using LibraryMS.DAL.UnitOfWork;
using Moq; 
namespace LibraryMS.Tests.BLL.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _sut;

        public BookServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _sut = new BookService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        private static Category CreateCategory()
        {
            return new Category("Fiction", "seed-user");
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                Title = "Clean Code",
                ISBN = "9780132350884",
                PublishedYear = 2008,
                TotalCopies = 5,
                CategoryId = 1,
                AuthorIds = new List<int>()
            };

            _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).ReturnsAsync((Category?)null);

            // Act
            Func<Task> act = async () => await _sut.CreateAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenIsbnAlreadyExists_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                Title = "Clean Code",
                ISBN = "9780132350884",
                PublishedYear = 2008,
                TotalCopies = 5,
                CategoryId = 1,
                AuthorIds = new List<int>()
            };

            _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).ReturnsAsync(CreateCategory());
            _unitOfWorkMock.Setup(u => u.Books.IsIsbnExistsAsync(dto.ISBN, null)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _sut.CreateAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                Title = "Clean Code",
                ISBN = "9780132350884",
                PublishedYear = 2008,
                TotalCopies = 5,
                CategoryId = 1,
                AuthorIds = new List<int> { 99 } // مؤلف مش موجود
            };

            _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).ReturnsAsync(CreateCategory());
            _unitOfWorkMock.Setup(u => u.Books.IsIsbnExistsAsync(dto.ISBN, null)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Authors.GetByIdAsync(99)).ReturnsAsync((Author?)null);

            // Act
            Func<Task> act = async () => await _sut.CreateAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ShouldAddBookAndSaveChanges()
        {
            // Arrange
            var dto = new CreateBookDto
            {
                Title = "Clean Code",
                ISBN = "9780132350884",
                PublishedYear = 2008,
                TotalCopies = 5,
                CategoryId = 1,
                AuthorIds = new List<int>()
            };

            _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(1)).ReturnsAsync(CreateCategory());
            _unitOfWorkMock.Setup(u => u.Books.IsIsbnExistsAsync(dto.ISBN, null)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<BookDto>(It.IsAny<Book>())).Returns(new BookDto());

            // Act
            await _sut.CreateAsync(dto, "librarian1");

            // Assert
            _unitOfWorkMock.Verify(u => u.Books.AddAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenBookNotFound_ShouldThrowNotFoundException()
        {
            // Arrange - هنا بنستخدم الـ Repository's DeleteAsync اللي بيرمي NotFoundException بنفسه
            _unitOfWorkMock.Setup(u => u.Books.DeleteAsync(1, "librarian1"))
                .ThrowsAsync(new NotFoundException("Book with id 1 not found."));

            // Act
            Func<Task> act = async () => await _sut.DeleteAsync(1, "librarian1");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}