using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface ILibraryService
    {
        void AddBookIntoLibrary(Book book);
        List<Book> GetAvailableBooks();
        int GetNumberOfAvailableBooksForSpecificBook(string title);
        Book RentABook(string title);
        float ReturnRentedBook(Book book);
    }
}
