using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    public class Book
    {
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public float RentPrice { get; set; }
        public bool IsBorrowed { get; set; }
        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }

        public Book(string bookTitle, string isbn, float rentPrice)
        {
            this.BookTitle = bookTitle;
            this.ISBN = isbn;
            this.RentPrice = rentPrice;
        }
    }
}
