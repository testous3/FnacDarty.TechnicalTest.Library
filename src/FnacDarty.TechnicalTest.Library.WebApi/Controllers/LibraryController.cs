using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using FnacDarty.TechnicalTest.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace FnacDarty.TechnicalTest.Library.Controllers
{
    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _bookService;

        public LibraryController(ILibraryService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("getAllBooks")]
        public IActionResult GetAllBooks()
        {
            var books = _bookService.GetAllBooks();

            return Ok(books);
        }

        [HttpPost("addBook")]
        public IActionResult AddBook([FromForm] AddBookRequest request)
        {
            _bookService.AddBook(request.Title, request.Author);

            return Ok();
        }
    }
}