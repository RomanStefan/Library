using System;
using System.Linq;
using Library;
using Library.Models;
using Library.Interfaces;
using FluentAssertions;
using NSubstitute;
using Xunit;


namespace LibraryTests
{
    public class LibraryServiceTests
    {
        private LibraryService libraryService;
        private IBookValidator bookValidatorFake;

        public LibraryServiceTests()
        {
            bookValidatorFake = Substitute.For<IBookValidator>();
            libraryService = new LibraryService(bookValidatorFake);
        }

        private void CreateTestLibrary()
        {

            //define the books from library
            Book book1 = new Book("title1","isbn1", 11);
            Book book2 = new Book("title2", "isbn2", 14);
            Book book3 = new Book("title3", "isbn3", 21);
            Book book4 = new Book("title4", "isbn4", 22);
            Book book5 = new Book("title1", "isbn1", 11);

            //add the books to library
            libraryService.AddBookIntoLibrary(book1);
            libraryService.AddBookIntoLibrary(book2);
            libraryService.AddBookIntoLibrary(book3);
            libraryService.AddBookIntoLibrary(book4);
            libraryService.AddBookIntoLibrary(book5);
        }

        [Fact]
        public void LibraryService_AddBookIntoLibrary_LibraryIsEmpty_BookIsAddedIntoLibrary()
        {
            // Arrange
            Book book1 = new Book("title1", "isbn1", 11);

            // Act
            libraryService.AddBookIntoLibrary(book1);

            // Assert 
            libraryService.library.Count().Should().Be(1);
        }

        [Fact]
        public void LibraryService_AddBookIntoLibrary_BookObjectIsNull_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            Book book1 = null;

            // Act
            var exception = Record.Exception(() => libraryService.AddBookIntoLibrary(book1));

            // Assert 
            exception.Message.Should().Be("Argument book cannot be null.");
        }

        [Fact]
        public void LibraryService_ReturnAvailableBooks_LibraryHaveSomeBooksRented_AvailableNumberOfBooksIsTheRightOne()
        {
            // Arrange
            CreateTestLibrary();
            libraryService.library[0].IsBorrowed = true;
            libraryService.library[1].IsBorrowed = true;


            // Act
            var availableBooks = libraryService.GetAvailableBooks();

            // Assert 
            availableBooks.Count().Should().Be(3);
        }

        [Fact]
        public void LibraryService_GetNumberOfAvailableBooksForSpecificBook_LibraryContainsTwoBooksForThisTitleButOneIsRented_ReturnedNumberShouldBe1()
        {
            // Arrange
            CreateTestLibrary();
            libraryService.library[0].IsBorrowed = true;

            // Act
            var numberOfAvailableBooks = libraryService.GetNumberOfAvailableBooksForSpecificBook("title1");

            // Assert 
            Assert.Equal(numberOfAvailableBooks, 1);
        }

        [Fact]
        public void LibraryService_GetNumberOfAvailableBooksForSpecificBook_LibraryDoNotContainExpectedBook_ReturnedNumberShouldBe0()
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var numberOfAvailableBooks = libraryService.GetNumberOfAvailableBooksForSpecificBook("title5");

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
            var exception = Record.Exception(() => libraryService.GetNumberOfAvailableBooksForSpecificBook(bookTitle));

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
            var rentedBook = libraryService.RentABook("title1");

            // Assert 
            Assert.Equal(rentedBook.BookTitle, expectedBookTitle);
        }

        [Fact]
        public void LibraryService_RentABook_RentABook_AvailableBooksAfterABookIsRentedIsTheRightNumber()
        {
            // Arrange
            CreateTestLibrary();
            var totalBooksAvailableBeforeRent = libraryService.GetAvailableBooks().Count;

            // Act
            var rentedBook = libraryService.RentABook("title1");

            // Assert 
            var totalBooksAvailableAfterRent = libraryService.GetAvailableBooks().Count;
            totalBooksAvailableAfterRent.Should().Be(totalBooksAvailableBeforeRent - 1);
        }

        [Fact]
        public void LibraryService_RentABook_RentABook_BookIsMarkedAsRented()
        {
            // Arrange
            CreateTestLibrary();

            // Act
            var rentedBook = libraryService.RentABook("title1");

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
            var rentedBook = libraryService.RentABook("title1");

            // Assert 
            var rentedStartDate = rentedBook.RentalStartDate.ToString("MM/dd/yyyy");
            Assert.Equal(rentedStartDate, expectedRentedStartDate);
        }

        [Fact]
        public void LibraryService_RentABook_BookTitleIsValidButBookIsAlreadyRented_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            CreateTestLibrary();
            libraryService.library[1].IsBorrowed = true;

            // Act
            var exception = Record.Exception(() => libraryService.RentABook("title2"));

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
            var exception = Record.Exception(() => libraryService.RentABook(bookTitle));

            // Assert 
            exception.Message.Should().Be("Book is not available");
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBook_IsBorrowedFieldIsChangedAfterReturnOfTheBook()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryService.library[1];
            rentedBook.IsBorrowed = true;

            // Act
            libraryService.ReturnRentedBook(rentedBook);

            // Assert 
            libraryService.library[1].IsBorrowed.Should().BeFalse();
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBookIn14DaysInterval_ReturnedPriceIsTheStandardPrice()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryService.library[1];
            rentedBook.IsBorrowed = true;
            rentedBook.RentalStartDate = DateTime.Today.AddDays(-10);

            // Act
            var price = libraryService.ReturnRentedBook(rentedBook);

            // Assert 
            price.Should().Be(rentedBook.RentPrice);
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_ReturnTheBookAfter14DaysInterval_ReturnedPriceIsTheRightOne()
        {
            // Arrange
            CreateTestLibrary();
            Book rentedBook = libraryService.library[1];
            rentedBook.IsBorrowed = true;
            rentedBook.RentalStartDate = DateTime.Today.AddDays(-20);

            // Act
            var price = libraryService.ReturnRentedBook(rentedBook);

            // Assert 
            price.Should().Be((float)14.84);
        }

        [Fact]
        public void LibraryService_ReturnRentedBook_BookObjectIsNull_ExceptionWithExpectedMessageIsThrow()
        {
            // Arrange
            Book book = null;

            // Act
            var exception = Record.Exception(() => libraryService.ReturnRentedBook(book));

            // Assert 
            exception.Message.Should().Be("Argument book cannot be null.");
        }
    }
}
