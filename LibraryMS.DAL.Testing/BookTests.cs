using LibraryMS.DAL.Entities;
using FluentAssertions;
namespace LibraryMS.DAL.Testing
{
    public class BookTests
    {
        private static Book CreateValidBook(int totalCopies = 5)
        {
            return new Book("Clean Code", "9780132350884", 2008, totalCopies, categoryId: 1, createdBy: "test-user");
        }

        [Fact]
        public void Constructor_WithValidData_ShouldSetAvailableCopiesEqualToTotalCopies()
        {
            // Arrange & Act
            var book = CreateValidBook(totalCopies: 10);

            // Assert
            book.AvailableCopies.Should().Be(10);
            book.TotalCopies.Should().Be(10);
        }

        [Fact]
        public void Constructor_WithEmptyTitle_ShouldThrowArgumentException()
        {
            // Act
            Action act = () => new Book("", "9780132350884", 2008, 5, 1, "test-user");

            // Assert
            act.Should().Throw<ArgumentException>()
                .WithMessage("*Title*");
        }

        [Fact]
        public void BorrowCopy_WhenCopiesAvailable_ShouldDecrementAvailableCopies()
        {
            // Arrange
            var book = CreateValidBook(totalCopies: 5);

            // Act
            book.BorrowCopy("librarian1");

            // Assert
            book.AvailableCopies.Should().Be(4);
        }

        [Fact]
        public void BorrowCopy_WhenNoCopiesAvailable_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var book = CreateValidBook(totalCopies: 1);
            book.BorrowCopy("librarian1"); // AvailableCopies بقت 0 دلوقتي

            // Act
            Action act = () => book.BorrowCopy("librarian1");

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*not available*");
        }

        [Fact]
        public void ReturnCopy_WhenBookIsBorrowed_ShouldIncrementAvailableCopies()
        {
            // Arrange
            var book = CreateValidBook(totalCopies: 5);
            book.BorrowCopy("librarian1"); // AvailableCopies بقت 4

            // Act
            book.ReturnCopy("librarian1");

            // Assert
            book.AvailableCopies.Should().Be(5);
        }

        [Fact]
        public void ReturnCopy_WhenAllCopiesAlreadyAvailable_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var book = CreateValidBook(totalCopies: 5); // كل النسخ متاحة أصلاً

            // Act
            Action act = () => book.ReturnCopy("librarian1");

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("*already available*");
        }
    }
}