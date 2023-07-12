using System;
using System.Collections.Generic;
using System.Linq;
using Library.Models;
using System.Text;
using System.Threading.Tasks;
using Library.Interfaces;

namespace Library
{
    public class LibraryService : ILibraryService
    {
        public List<Book> library { get; set; }

        public LibraryService()
        {
            this.library = new List<Book>();
        }

        public void AddBookIntoLibrary(Book book)
        {
            #region Safe Guards
            if (book == null)
                throw new Exception("Argument book cannot be null.");
            #endregion

            library.Add(book);
        }

        public List<Book> GetAvailableBooks()
        {
            return library.Where(book => book.IsBorrowed == false).ToList();
        }

        public int GetNumberOfAvailableBooksForSpecificBook(string title)
        {
            #region Safe Guards
            if (string.IsNullOrWhiteSpace(title))
                throw new Exception("Argument title cannot be null or whitespaces.");
            #endregion

            return library.Where(book => book.BookTitle == title && book.IsBorrowed == false).Count();
        }

        public Book RentABook(string title)
        {

            List<Book> availableBooks = library.Where(book => book.BookTitle == title && book.IsBorrowed == false).ToList();

            if(availableBooks == null)
            {
                throw new Exception("Book is not available");
            }

            Book selectedBook = availableBooks.First();
            selectedBook.IsBorrowed = true;
            selectedBook.RentalStartDate = DateTime.Now;

            return selectedBook;
        }

        public float ReturnRentedBook(Book book)
        {
            book.IsBorrowed = false;

            return ComputeRentalPrice(book);
            
        }

        private float ComputeRentalPrice(Book book)
        {
            DateTime RentalEndDate = DateTime.Now;
            int rentaldays = (RentalEndDate - book.RentalStartDate).Days;

            if (rentaldays > 14)
                return book.RentPrice + (rentaldays - 14) / 100 * book.RentPrice;

            return book.RentPrice;
        }
    }
}
