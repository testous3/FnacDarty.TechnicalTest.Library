using FnacDarty.TechnicalTest.Library.Domain.Entities;
using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.TechnicalTest.Library.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }
        public void AddLoan(int customerId, int bookId)
        {
            var currentDate = DateTime.Now;
            var allLoans = _loanRepository.Get();
            var id = allLoans.Count == 0 ? 1 : allLoans.Max(b => b.Id) + 1;
            var loan = new Loan(0, customerId, bookId, currentDate, currentDate.AddDays(21), null);
            _loanRepository.AddLoan(loan);

        }

        public List<Loan> GetAllActiveLoans()
        {
            return _loanRepository.Get().Where(l => !l.ReturnedAt.HasValue).ToList();
        }
    }
}
