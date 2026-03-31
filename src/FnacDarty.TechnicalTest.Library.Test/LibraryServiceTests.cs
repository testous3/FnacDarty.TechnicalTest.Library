using Castle.Core.Resource;
using FnacDarty.TechnicalTest.Library.Domain.Entities;
using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using FnacDarty.TechnicalTest.Library.Domain.Services;
using FnacDarty.TechnicalTest.Library.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace FnacDarty.TechnicalTest.Library.Test
{

    public class LibraryServiceTests
    {

        [Fact]
        public void Borrow_Should_Return_NOT_FOUND_When_Book_Is_NotFound()
        {

            var bookRepoMock = new Mock<IBookRepository>();
            var loanServiceMock = new Mock<ILoanService>();
            var customerServiceMock = new Mock<ICustomerService>();
            var customerId = 1;
            var bookId = 10;

            bookRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Book>() { new Book(bookId, string.Empty, string.Empty) });

            loanServiceMock
                .Setup(x => x.GetAllActiveLoans())
                .Returns(new List<Loan>());

            var service = new LibraryService(
                bookRepoMock.Object,
                customerServiceMock.Object,
                loanServiceMock.Object
            );

            var result = service.BorrowBooks(customerId, new List<int> { 11 });
            Assert.Empty(result.BorrowedBooks);
            Assert.Single(result.RejectedBooks);
            Assert.Equal(result.RejectedBooks.Single().ReasonCode, "NOT_FOUND");

        }

        [Fact]
        public void Borrow_Should_Return_NOT_AVAILABLE_When_Book_Is_NotAvailable()
        {

            var bookRepoMock = new Mock<IBookRepository>();
            var loanServiceMock = new Mock<ILoanService>();
            var customerServiceMock = new Mock<ICustomerService>();
            var customerId = 1;
            var bookId = 10;

            bookRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Book>() { new Book(bookId, string.Empty, string.Empty) });

            loanServiceMock
                .Setup(x => x.GetAllActiveLoans())
                .Returns(new List<Loan>() { new Loan(100, customerId, bookId, DateTime.Now, DateTime.Now.AddDays(21), null) });

            var service = new LibraryService(
                bookRepoMock.Object,
                customerServiceMock.Object,
                loanServiceMock.Object
            );

            var result = service.BorrowBooks(customerId, new List<int> { bookId });
            Assert.Empty(result.BorrowedBooks);
            Assert.Single(result.RejectedBooks);
            Assert.Equal(result.RejectedBooks.Single().ReasonCode, "NOT_AVAILABLE");

        }


        [Fact]
        public void Borrow_Should_Return_LOAN_OVERDUE_When_Book_Is_OverDue()
        {

            var bookRepoMock = new Mock<IBookRepository>();
            var loanServiceMock = new Mock<ILoanService>();
            var customerServiceMock = new Mock<ICustomerService>();
            var customerId = 1;
            var bookId = 10;

            bookRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Book>() { new Book(bookId, string.Empty, string.Empty) });

            loanServiceMock
                .Setup(x => x.GetAllActiveLoans())
                .Returns(new List<Loan>() { new Loan(100, customerId, bookId, DateTime.Now, DateTime.Now.AddDays(-10), null) });

            var service = new LibraryService(
                bookRepoMock.Object,
                customerServiceMock.Object,
                loanServiceMock.Object
            );

            var result = service.BorrowBooks(customerId, new List<int> { bookId });
            Assert.Empty(result.BorrowedBooks);
            Assert.Single(result.RejectedBooks);
            Assert.Equal(result.RejectedBooks.Single().ReasonCode, "LOAN_OVERDUE");

        }

        [Fact]
        public void Borrow_Should_Borrow_When_Books_Are_Available()
        {

            var bookRepoMock = new Mock<IBookRepository>();
            var loanServiceMock = new Mock<ILoanService>();
            var customerServiceMock = new Mock<ICustomerService>();
            var customerId = 1;


            bookRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Book>() {

                    new Book(10, string.Empty, string.Empty),
                    new Book(11, string.Empty, string.Empty),
                    new Book(12, string.Empty, string.Empty),
                });

            loanServiceMock
                .Setup(x => x.GetAllActiveLoans())
                .Returns(new List<Loan>() { });

            var service = new LibraryService(
                bookRepoMock.Object,
                customerServiceMock.Object,
                loanServiceMock.Object
            );

            var result = service.BorrowBooks(customerId, new List<int> { 10, 11, 12 });
            Assert.Empty(result.RejectedBooks);
            Assert.NotEmpty(result.BorrowedBooks);
            Assert.Equal(result.BorrowedBooks.Count, 3);

        }


        [Fact]
        public void Borrow_Should_Borrow_When_Books_Are_Available_AND_GREAT3()
        {

            var bookRepoMock = new Mock<IBookRepository>();
            var loanServiceMock = new Mock<ILoanService>();
            var customerServiceMock = new Mock<ICustomerService>();
            var customerId = 1;


            bookRepoMock
                .Setup(x => x.GetAll())
                .Returns(new List<Book>() {

                    new Book(10, string.Empty, string.Empty),
                    new Book(11, string.Empty, string.Empty),
                    new Book(12, string.Empty, string.Empty),
                    new Book(13, string.Empty, string.Empty),
                });

            loanServiceMock
                .Setup(x => x.GetAllActiveLoans())
                .Returns(new List<Loan>() { });

            var service = new LibraryService(
                bookRepoMock.Object,
                customerServiceMock.Object,
                loanServiceMock.Object
            );

            var result = service.BorrowBooks(customerId, new List<int> { 10, 11, 12, 13 });
            Assert.NotEmpty(result.RejectedBooks);
            Assert.NotEmpty(result.BorrowedBooks);
            Assert.Equal(result.BorrowedBooks.Count, 3);
            Assert.Equal(result.RejectedBooks.Count, 1);
            Assert.Equal(result.RejectedBooks.Single().ReasonCode, "LIMIT_REACHED");

        }
    }
}
