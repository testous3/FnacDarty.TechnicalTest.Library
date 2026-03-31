namespace FnacDarty.TechnicalTest.Library.WebApi.Models
{
    public class BorrowBooksModel
    {

        public int CustomerId { get; set; }

        public List<int> BookIds { get; set; }
    }
}
