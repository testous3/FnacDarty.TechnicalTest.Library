namespace FnacDarty.TechnicalTest.Library.Models
{
    public class AddBookRequest
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public AddBookRequest(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public AddBookRequest() { }


    }
}