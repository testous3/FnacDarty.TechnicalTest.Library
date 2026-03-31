using System;
using System.Collections.Generic;
using System.Text;

namespace FnacDarty.TechnicalTest.Library.Domain.Entities
{
    public class BorrowResult
    {
        public List<BorrowedBooksModel> BorrowedBooks = new List<BorrowedBooksModel>();
        public List<RejectedBooksModel> RejectedBooks = new List<RejectedBooksModel>();

    }

    public class BorrowedBooksModel
    {
        public int BookId { get; set; }
        public DateTime DueAt { get; set; }
    }

    public class RejectedBooksModel
    {
        public int BookId { get; set; }
        public string ReasonCode { get; set; }

        public string reasonLabel { get; set; }
    }
}
