using AutoMapper;
using FluentAssertions;
using LibraryMS.BLL.DTOs.Member;
using LibraryMS.BLL.Service.Implementation;
using LibraryMS.DAL.Entities;
using LibraryMS.DAL.Exceptions;
using LibraryMS.DAL.UnitOfWork;
using Moq;
namespace LibraryMS.BLL.Testing.Services
{ 
    public class MemberServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly MemberService _sut;

        public MemberServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _sut = new MemberService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenEmailAlreadyExists_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                FullName = "Ahmed Ali",
                Email = "ahmed@test.com",
                PhoneNumber = "01012345678"
            };

            _unitOfWorkMock.Setup(u => u.Members.IsEmailExistsAsync(dto.Email!, null)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _sut.CreateAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*already exists*");
        }

        [Fact]
        public async Task CreateAsync_WhenPhoneAlreadyExists_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                FullName = "Ahmed Ali",
                Email = "ahmed@test.com",
                PhoneNumber = "01012345678"
            };

            _unitOfWorkMock.Setup(u => u.Members.IsEmailExistsAsync(dto.Email!, null)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.Members.IsPhoneExistsAsync(dto.PhoneNumber, null)).ReturnsAsync(true);

            // Act
            Func<Task> act = async () => await _sut.CreateAsync(dto, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*phone number*");
        }

        [Fact]
        public async Task DeleteAsync_WhenMemberHasActiveBorrows_ShouldThrowBusinessRuleException()
        {
            // Arrange
            var member = new Member("Ahmed Ali", "ahmed@test.com", "01012345678", DateTime.UtcNow, "seed-user");
            var borrowRecord = new BorrowRecord(1, 1, DateTime.UtcNow.AddDays(14), null, "librarian1");

             member.BorrowRecords.Add(borrowRecord); 

            _unitOfWorkMock.Setup(u => u.Members.GetByIdWithBorrowRecordsAsync(1)).ReturnsAsync(member);

            // Act
            Func<Task> act = async () => await _sut.DeleteAsync(1, "librarian1");

            // Assert
            await act.Should().ThrowAsync<BusinessRuleException>()
                .WithMessage("*active borrow*");
        }

        [Fact]
        public async Task DeleteAsync_WhenMemberHasNoActiveBorrows_ShouldSucceed()
        {
            // Arrange
            var member = new Member("Ahmed Ali", "ahmed@test.com", "01012345678", DateTime.UtcNow, "seed-user");

            _unitOfWorkMock.Setup(u => u.Members.GetByIdWithBorrowRecordsAsync(1)).ReturnsAsync(member);

            // Act
            Func<Task> act = async () => await _sut.DeleteAsync(1, "librarian1");

            // Assert
            await act.Should().NotThrowAsync();
            _unitOfWorkMock.Verify(u => u.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task ActivateAsync_WhenMemberNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Members.GetByIdAsync(1)).ReturnsAsync((Member?)null);

            // Act
            Func<Task> act = async () => await _sut.ActivateAsync(1, "librarian1");

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}