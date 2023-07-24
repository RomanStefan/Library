using System;
using System.Linq;
using Library;
using Library.Models;
using FluentAssertions;
using Xunit;

namespace LibraryTests
{
    public class LibraryServiceTests
    {
        private LibraryService libraryServiceFake;

        public LibraryServiceTests()
        {
            libraryServiceFake = new LibraryService();
        }

        private void CreateTestLibrary()
        {

            //define the books from library
            Book book1 = new Book("title1", "isbn1", 11);
            Book book2 = new Book("title2", "isbn2", 14);
            Book book3 = new Book("title3", "isbn3", 21);
            Book book4 = new Book("title4", "isbn4", 22);
            Book book5 = new Book("title1", "isbn1", 11);

            //add the books to library
            libraryServiceFake.AddBookIntoLibrary(book1);
            libraryServiceFake.AddBookIntoLibrary(book2);
            libraryServiceFake.AddBookIntoLibrary(book3);
            libraryServiceFake.AddBookIntoLibrary(book4);
            libraryServiceFake.AddBookIntoLibrary(book5);
        }

        [Fact]
        public void LibraryService_AddBookIntoLibrary_LibraryIsEmpty_BookIsAddedIntoLibrary()
        {
            // Arrange
            Book book1 = new Book("title1", "isbn1", 11);

            // Act
            libraryServiceFake.AddBookIntoLibrary(book1);

            // Assert 
            libraryServiceFake.library.Count().Should().Be(1);
        }

        [Fact]
        public void LibraryService_AddBookIntoLibrary_BookObjectIsNull_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            Book book1 = null;

            // Act
            var exception = Record.Exception(() => libraryServiceFake.AddBookIntoLibrary(book1));

            // Assert 
            exception.Message.Should().Be("Argument book cannot be null.");
        }

        [Fact]
        public void LibraryService_ReturnAvailableBooks_LibraryHaveSomeBooksRented_AvailableNumberOfBooksIsTheRightOne()
        {
            // Arrange
            CreateTestLibrary();
            libraryServiceFake.library[0].IsBorrowed = true;
            libraryServiceFake.library[1].IsBorrowed = true;


            // Act
            var availableBooks = libraryServiceFake.GetAvailableBooks();

            // Assert 
            availableBooks.Count().Should().Be(3);
        }

        [Fact]
        public void LibraryService_GetNumberOfAvailableBooksForSpecificBook_LibraryContainsTwoBooksForThisTitleButOneIsRented_ReturnedNumberShouldBe1()
        {
            // Arrange
            CreateTestLibrary();
            libraryServiceFake.library[0].IsBorrowed = true;

            // Act
            var numberOfAvailableBooks = libraryServiceFake.GetNumberOfAvailableBooksForSpecificBook("title1");

            // Assert 
            Assert.Equal(numberOfAvailableBooks, 1);
        }

        [Fact]
        public void LibraryService_GetNumberOfAvailableBooksForSpecificBook_LibraryDoNotContainExpectedBook_ReturnedNumberShouldBe0()
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var numberOfAvailableBooks = libraryServiceFake.GetNumberOfAvailableBooksForSpecificBook("title5");

            // Assert 
            Assert.Equal(numberOfAvailableBooks, 0);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void LibraryService_GetNumberOfAvailableBooksForSpecificBook_BookTitleIsNullOrWhitespaces_ExceptionWithExpectedMessageIsThrow(string bookTitle)
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var exception = Record.Exception(() => libraryServiceFake.GetNumberOfAvailableBooksForSpecificBook(bookTitle));

            // Assert 
            exception.Message.Should().Be("Argument title cannot be null or whitespaces.");
        }

        [Fact]
        public void LibraryService_RentABook_LibraryContainExpectedBook_RentedBookTitleShouldBeExpectedOne()
        {
            // Arrange
            CreateTestLibrary();
            var expectedBookTitle = "title1";

            // Act
            var rentedBook = libraryServiceFake.RentABook("title1");

            // Assert 
            Assert.Equal(rentedBook.BookTitle, expectedBookTitle);
        }

        [Fact]
        public void LibraryService_RentABook_RentABook_AvailableBooksAfterABookIsRentedIsTheRightNumber()
        {
            // Arrange
            CreateTestLibrary();
            var totalBooksAvailableBeforeRent = libraryServiceFake.GetAvailableBooks().Count;

            // Act
            var rentedBook = libraryServiceFake.RentABook("title1");

            // Assert 
            var totalBooksAvailableAfterRent = libraryServiceFake.GetAvailableBooks().Count;
            totalBooksAvailableAfterRent.Should().Be(totalBooksAvailableBeforeRent - 1);
        }

        [Fact]
        public void LibraryService_RentABook_RentABook_BookIsMarkedAsRented()
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var rentedBook = libraryServiceFake.RentABook("title1");

            // Assert 
            rentedBook.IsBorrowed.Should().BeTrue();
        }

        [Fact]
        public void LibraryService_RentABook_RentABook_RentedStartdayIsExpectedOne()
        {
            // Arrange
            CreateTestLibrary();
            string expectedRentedStartDate = DateTime.Now.ToString("MM/dd/yyyy");

            // Act
            var rentedBook = libraryServiceFake.RentABook("title1");

            // Assert 
            var rentedStartDate = rentedBook.RentalStartDate.ToString("MM/dd/yyyy");
            Assert.Equal(rentedStartDate, expectedRentedStartDate);
        }

        [Fact]
        public void LibraryService_RentABook_BookTitleIsValidButBookIsAlreadyRented_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            CreateTestLibrary();
            libraryServiceFake.library[1].IsBorrowed = true;

            // Act
            var exception = Record.Exception(() => libraryServiceFake.RentABook("title2"));

            // Assert 
            exception.Message.Should().Be("Book is not available");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void LibraryService_RentABook_BookTitleIsNullOrWhitespaces_ExceptionWithExpectedMessageIsThrow(string bookTitle)
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var exception = Record.Exception(() => libraryServiceFake.RentABook(bookTitle));

            // Assert 
            exception.Message.Should().Be("Book is not available");
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBook_IsBorrowedFieldIsChangedAfterReturnOfTheBook()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryServiceFake.library[1];
            rentedBook.IsBorrowed = true;

            // Act
            libraryServiceFake.ReturnRentedBook(rentedBook);

            // Assert 
            libraryServiceFake.library[1].IsBorrowed.Should().BeFalse();
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBookIn14DaysInterval_ReturnedPriceIsTheStandardPrice()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryServiceFake.library[1];
            rentedBook.IsBorrowed = true;
            rentedBook.RentalStartDate = DateTime.Today.AddDays(-10);

            // Act
            var price = libraryServiceFake.ReturnRentedBook(rentedBook);

            // Assert 
            price.Should().Be(rentedBook.RentPrice);
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBookAfter14DaysInterval_ReturnedPriceIsTheRightOne()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryServiceFake.library[1];
            rentedBook.IsBorrowed = true;
            rentedBook.RentalStartDate = DateTime.Today.AddDays(-20);

            // Act
            var price = libraryServiceFake.ReturnRentedBook(rentedBook);

            // Assert 
            price.Should().Be((float)14.84);
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_BookObjectIsNull_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            Book book = null;

            // Act
            var exception = Record.Exception(() => libraryServiceFake.ReturnRentedBook(book));

            // Assert 
            exception.Message.Should().Be("Argument book cannot be null.");
        }
    }
}
