using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Library.Interfaces;
using Library.Models;
using NSubstitute;
using Xunit;

namespace LibraryTests
{
    public class LibraryServiceTests
    {
        private ILibraryService libraryServiceFake; 

        public LibraryServiceTests()
        {
            libraryServiceFake = Substitute.For<ILibraryService>();
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
            var availableBooks = libraryServiceFake.ReturnAvailableBooks();
            libraryServiceFake.ReturnAvailableBooks().Count().Should().Be(1);
        }
    }
}
