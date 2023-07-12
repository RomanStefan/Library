using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            //define the library
            var library = new LibraryService();

            //define the books from library
            Book book1 = new Book("title1", "isbn1", 11);
            Book book2 = new Book("title2", "isbn2", 14);
            Book book3 = new Book("title3", "isbn3", 21);
            Book book4 = new Book("title4", "isbn4", 22);
            Book book5 = new Book("title1", "isbn1", 11);

            //add the books to library
            library.AddBookIntoLibrary(book1);
            library.AddBookIntoLibrary(book2);
            library.AddBookIntoLibrary(book3);
            library.AddBookIntoLibrary(book4);
            library.AddBookIntoLibrary(book5);

            //test the functionality to return the available books
            library.GetAvailableBooks();

            //test the functionality to return number of available books for "title1" 
            library.GetNumberOfAvailableBooksForSpecificBook("title1");


            //borrow the book "title1"
            var rentedBook = library.RentABook("title1");
            //test the functionality to return the available books after we rent the book "title1"
            library.GetAvailableBooks();
            //test the functionality to return number of available books for "title1"  after we rent the book "title1" 
            library.GetNumberOfAvailableBooksForSpecificBook("title1");

            //Return the rented book
            library.ReturnRentedBook(rentedBook);
        }
    }
}
