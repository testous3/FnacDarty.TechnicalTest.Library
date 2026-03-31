using FnacDarty.TechnicalTest.Library.Domain.Interfaces;
using FnacDarty.TechnicalTest.Library.Infrastructure.Repositories;
using FnacDarty.TechnicalTest.Library.Models;
using FnacDarty.TechnicalTest.Library.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FnacDarty.TechnicalTest.Library.Controllers
{
    [ApiController]
    [Route("api/library")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _bookService;
        private readonly ICustomerService _customerService;

        public LibraryController(ILibraryService bookService, ICustomerService customerService)
        {
            _bookService = bookService;
            _customerService = customerService;
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

        [HttpPost("borrow")]
        public IActionResult Borrow([FromForm] BorrowBooksModel request)
        {
            try
            {

                if (!_customerService.IsCustomerExists(request.CustomerId))
                    return NotFound("Le client est introuvable");

                var result = _bookService.BorrowBooks(request.CustomerId, request.BookIds);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Une erreur inattendue est survenue, veuillez réessayer ultérieurement.");
            }


        }
    }
}