using FnacDarty.TechnicalTest.Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.TechnicalTest.Library.Domain.Interfaces
{
    public interface ILoanRepository
    {
        void AddLoan(Loan loan);
        IReadOnlyCollection<Loan> Get();
    }
}