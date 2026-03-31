using FnacDarty.TechnicalTest.Library.Domain.Entities;
using FnacDarty.TechnicalTest.Library.Domain.Interfaces;

namespace FnacDarty.TechnicalTest.Library.Domain.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICustomerService _customerService;
        private readonly ILoanService _loanService;
        public LibraryService(IBookRepository bookRepository, ICustomerService customerService, ILoanService loanService)
        {
            _bookRepository = bookRepository;
            _customerService = customerService;
            _loanService = loanService;
        }

        public IReadOnlyCollection<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }

        public void AddBook(string title, string author)
        {
            var allBooks = _bookRepository.GetAll();
            var id = allBooks.Count == 0 ? 1 : allBooks.Max(b => b.Id) + 1;
            var book = new Book(id, title, author);

            _bookRepository.Add(book);
        }



        public BorrowResult BorrowBooks(int customerId, List<int> bookIds)
        {
            var currentDate = DateTime.Now;
            var borrowResult = new BorrowResult();
            var allBooks = _bookRepository.GetAll();
            var currentLoans = _loanService.GetAllActiveLoans();

            var currentLoansWithNotOverDueDate = currentLoans.Where(c => c.DueAt > currentDate).ToList();

            var currentLoansWithOverDueDate = currentLoans.Where(c => c.DueAt <= currentDate).ToList();

            var notFoundBooks = bookIds.Where(b => !allBooks.Select(a => a.Id).Contains(b)).ToList();
            var notAvailableBooks = bookIds.Where(b => currentLoansWithNotOverDueDate.Select(c => c.BookId).Contains(b)).ToList();

            var overDueBooks = bookIds.Where(b => currentLoansWithOverDueDate.Select(c => c.BookId).Contains(b)).ToList();

            var restBooks = bookIds.Except(notFoundBooks).Except(notAvailableBooks).Except(overDueBooks).ToList();


            foreach (var notFound in notFoundBooks)
            {
                borrowResult.RejectedBooks.Add(new RejectedBooksModel() { BookId = notFound, ReasonCode = "NOT_FOUND", reasonLabel = "Le livre est introuvable" });
            }

            foreach (var notAvailabl in notAvailableBooks)
            {
                borrowResult.RejectedBooks.Add(new RejectedBooksModel() { BookId = notAvailabl, ReasonCode = "NOT_AVAILABLE", reasonLabel = "Le livre n'est pas disponible actuellement" });
            }


            foreach (var overDue in overDueBooks)
            {
                borrowResult.RejectedBooks.Add(new RejectedBooksModel() { BookId = overDue, ReasonCode = "LOAN_OVERDUE", reasonLabel = "La date d'échéance d'un prêt est dépassé" });
            }

            for (int i = 0; i < restBooks.Count; i++)
            {
                if (restBooks.Count > 3)
                {
                    borrowResult.RejectedBooks.Add(new RejectedBooksModel() { BookId = restBooks[i], ReasonCode = "LIMIT_REACHED", reasonLabel = "Le client a atteint la limite de 3 emprunts simultanés" });

                }
                else
                {
                    _loanService.AddLoan(customerId, restBooks[i]);
                    borrowResult.BorrowedBooks.Add(new BorrowedBooksModel() { BookId = restBooks[i], DueAt = DateTime.Now.AddDays(21) });
                }
            }

            if (restBooks.Count > 3)
            {
                borrowResult.RejectedBooks.Add(new RejectedBooksModel() { ReasonCode = "LIMIT_REACHED  ", reasonLabel = "Le livre n'est pas disponible actuellement" });

            }


            return borrowResult;
        }
    }
}
