using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library;
using Library.Interfaces;
using Library.Models;
using NSubstitute;
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
    }
}
