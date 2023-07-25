using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Interfaces;
using Library.Models;
using FluentValidation;
using FluentValidation.Results;


namespace Library.Validator
{
    public class BookValidator : AbstractValidator<Book>, IBookValidator
    {
        public BookValidator()
        {
            RuleFor(book => book.BookTitle).NotNull().WithMessage("BookTitle cannot be empty");
            RuleFor(book => book.ISBN).NotNull().WithMessage("ISBN cannot be empty");
            RuleFor(book => book.RentPrice).NotNull().WithMessage("RentPrice cannot be empty");
        }

        public ValidationResult ValidateBook(Book book)
        {
            var failures = new List<ValidationFailure>();

            var result = Validate(book);

            if (result.IsValid == false)
            {
                var validationMessage = BuildMessageForFailedBook(book, result);
                var failure = new ValidationFailure(book.BookTitle, validationMessage);

                failures.Add(failure);
            }

            return new ValidationResult(failures);
        }

        private string BuildMessageForFailedBook(Book book, ValidationResult result)
        {
            var message = new StringBuilder("");
            var resultErrorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            message.Append($"The following issues were found in book {book.BookTitle}: ");
            message.Append(string.Join(" ", resultErrorMessages));

            return message.ToString();
        }
    }
}
