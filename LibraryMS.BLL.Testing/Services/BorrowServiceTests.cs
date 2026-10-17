using AutoMapper;
using FluentAssertions;
using LibraryMS.BLL.DTOs.BorrowRecord;
using LibraryMS.BLL.Service.Implementation;
using LibraryMS.DAL.Entities;
using LibraryMS.DAL.Exceptions;
using LibraryMS.DAL.UnitOfWork;
using Moq;
using Member = LibraryMS.DAL.Entities.Member;
namespace LibraryMS.BLL.Testing.Services
{
    public class BorrowServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BorrowService _sut; // sut = System Under Test

        public BorrowServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _sut = new BorrowService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        private static Book CreateBook(int availableCopies = 5)
        {
             var totalCopies = Math.Max(availableCopies, 1);
            var book = new Book("Clean Code", "9780132350884", 2008, totalCopies, 1, "seed-user");

             var copiesToConsume = totalCopies - availableCopies;
            for (int i = 0; i < copiesToConsume; i++)
            {
                book.BorrowCopy("seed-user");
            }

            return book;
        }
        private static Member CreateActiveMember()
        {
            return new Member("Ahmed Ali", "ahmed@test.com", "01012345678", DateTime.Now, "seed-user");
        }

        [Fact]
        public async Task BorrowBookAsync_WhenBookNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var dto = new CreateBorrowDto { BookId = 1, MemberId = 1, DueDate = DateTime.Now.AddDays(14) };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(1))
                .ReturnsAsync((Book?)null); 

            // Act
            Func<Task> act = async () => await _sut.BorrowBookAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }       

        [Fact]
        public async Task BorrowBookAsync_WhenBookHasNoAvailableCopies_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var book = CreateBook(availableCopies: 0);
            var member = CreateActiveMember();
            var dto = new CreateBorrowDto { BookId = 1, MemberId = 1, DueDate = DateTime.Now.AddDays(14) };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(1)).ReturnsAsync(member);
            _unitOfWorkMock.Setup(u => u.BorrowRecords.HasOverdueBooksAsync(1)).ReturnsAsync(false);

            // Act
            Func<Task> act = async () => await _sut.BorrowBookAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task BorrowBookAsync_WhenMemberIsInactive_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var book = CreateBook(availableCopies: 5);
            var member = CreateActiveMember();
            member.Deactivate("admin"); 

            var dto = new CreateBorrowDto { BookId = 1, MemberId = 1, DueDate = DateTime.Now.AddDays(14) };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(1)).ReturnsAsync(member);

            // Act
            Func<Task> act = async () => await _sut.BorrowBookAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*not active*");
        }

        [Fact]
        public async Task BorrowBookAsync_WithValidData_ShouldDecrementBookCopiesAndSaveChanges()
        {
            // Arrange
            var book = CreateBook(availableCopies: 5);
            var member = CreateActiveMember();
            var dto = new CreateBorrowDto { BookId = 1, MemberId = 1, DueDate = DateTime.Now.AddDays(14) };

            _unitOfWorkMock.Setup(u => u.Books.GetByIdAsync(1)).ReturnsAsync(book);
            _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(1)).ReturnsAsync(member);
            _unitOfWorkMock.Setup(u => u.BorrowRecords.HasOverdueBooksAsync(1)).ReturnsAsync(false);
            _mapperMock.Setup(m => m.Map<BorrowRecordDto>(It.IsAny<BorrowRecord>()))
                .Returns(new BorrowRecordDto());

            // Act
            await _sut.BorrowBookAsync(dto, "librarian1");

            // Assert
            book.AvailableCopies.Should().Be(4);
            _unitOfWorkMock.Verify(u => u.BorrowRecords.AddAsync(It.IsAny<BorrowRecord>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task ReturnBookAsync_WhenAlreadyReturned_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var book = CreateBook(availableCopies: 4);
            var record = new BorrowRecord(1, 1, DateTime.Now.AddDays(14), null, "librarian1");
            record.ReturnBook(DateTime.Now, null, "librarian1"); 

            _unitOfWorkMock.Setup(u => u.BorrowRecords.GetByIdWithDetailsAsync(1)).ReturnsAsync(record);

            var dto = new ReturnBookDto();

            // Act
            Func<Task> act = async () => await _sut.ReturnBookAsync(1, dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*already been returned*");
        }
    }
}