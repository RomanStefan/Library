using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Library.Models;

namespace Library.Validator
{
    public class BookValidator :AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.BookTitle).NotNull().WithMessage("BookTitle cannot be empty");
            RuleFor(book => book.ISBN).NotNull().WithMessage("ISBN cannot be empty");
            RuleFor(book => book.RentPrice).NotNull().WithMessage("RentPrice cannot be empty");
        }
    }
}
