using FnacDarty.TechnicalTest.Library.Domain.Entities;
using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace FnacDarty.TechnicalTest.Library.Infrastructure.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        public List<Loan> Loans;

        public LoanRepository()
        {
            Loans = new List<Loan>();
        }
        public void AddLoan(Loan loan)
        {
            Loans.Add(loan);
        }

        public IReadOnlyCollection<Loan> Get()
        {
            return Loans.AsReadOnly();
        }
    }
}
