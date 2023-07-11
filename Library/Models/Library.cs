﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Library
    {
        private List<Book> library { get; set; }

        public void AddBookIntoLibrary(Book book)
        {
            library.Add(book);
        }

        public List<Book> ReturnAvailableBooks()
        {
            return library.Where(book => book.IsBorrowed == false).ToList();
        }

        public int ReturnNumberOfAvailableBooksForSpecificBook(string title)
        {
            return library.Where(book => book.BookTitle == title && book.IsBorrowed == false).Count();
        }

        public Book BorrowBook(string title)
        {

            List<Book> availableBooks = library.Where(book => book.BookTitle == title && book.IsBorrowed == false).ToList();

            if(availableBooks == null)
            {
                throw new Exception("Book is not available");
            }

            Book selectedBook = availableBooks.Select(book => book).Single();
            selectedBook.IsBorrowed = true;
            selectedBook.RentalStartDate = DateTime.Now;

            return selectedBook;
        }

        public float ReturnRentedBook(Book book)
        {
            book.IsBorrowed = false;
            AddBookIntoLibrary(book);

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
