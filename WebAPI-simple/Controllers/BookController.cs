using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.CustomActionFilter;
using WebAPI_simple.Data;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;
using System.Text.RegularExpressions;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IBookRepository _bookRepository;

        public BooksController(AppDbContext dbContext, IBookRepository bookRepository)
        {
            _dbContext = dbContext;
            _bookRepository = bookRepository;
        }

        [HttpGet("get-all-books")]
        [Authorize(Roles = "Read")]
        public IActionResult GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] string? sortBy, [FromQuery] bool isAscending = true,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            // su dung repository pattern
            var allBooks = _bookRepository.GetAllBooks(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(allBooks);
        }

        [HttpGet]
        [Route("get-book-by-id/{id}")]
        [Authorize(Roles = "Read")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            var bookWithIdDTO = _bookRepository.GetBookById(id);
            if (bookWithIdDTO == null)
            {
                return NotFound(new { message = "Không tìm thấy sách" });
            }
            return Ok(bookWithIdDTO);
        }

        [HttpPost("add-book")]
        [ValidateModel]
        [Authorize(Roles = "Write")]
        public IActionResult AddBook([FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            // validate request
            if (ValidateAddBook(addBookRequestDTO))
            {
                var bookAdd = _bookRepository.AddBook(addBookRequestDTO);
                return Ok(bookAdd);
            }
            return BadRequest(ModelState);
        }

        [HttpPut("update-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult UpdateBookById(int id, [FromBody] AddBookRequestDTO bookDTO)
        {
            if (!ValidateAddBook(bookDTO))
            {
                return BadRequest(ModelState);
            }
            var updateBook = _bookRepository.UpdateBookById(id, bookDTO);
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]
        [Authorize(Roles = "Write")]
        public IActionResult DeleteBookById(int id)
        {
            var deleteBook = _bookRepository.DeleteBookById(id);
            return Ok(deleteBook);
        }
        #region Private methods
        private bool ValidateAddBook(AddBookRequestDTO addBookRequestDTO)
        {
            if (addBookRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO), "Please add book data");
                return false;
            }
            //1.Title không được rỗng
            if (string.IsNullOrWhiteSpace(addBookRequestDTO.Title))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Title),
                    $"{nameof(addBookRequestDTO.Title)} cannot be empty");
            }
            //1.Title không được chứa ký tự đặc biệt (chỉ cho phép chữ, số, khoảng trắng)
            else if (!Regex.IsMatch(addBookRequestDTO.Title, @"^[\p{L}\p{N}\s]+$"))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Title),
                    $"{nameof(addBookRequestDTO.Title)} cannot contain special characters");
            }
            //ktra Description NotNull
            if (string.IsNullOrEmpty(addBookRequestDTO.Description))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Description),
                    $"{nameof(addBookRequestDTO.Description)} cannot be null");
            }
            //ktra rating (0,5)
            if (addBookRequestDTO.Rate < 0 || addBookRequestDTO.Rate > 5)
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.Rate),
                    $"{nameof(addBookRequestDTO.Rate)} cannot be less than 0 and more than 5");
            }
            //4.PublisherID phải tồn tại trong bảng Publishers
            if (!_dbContext.Publishers.Any(p => p.Id == addBookRequestDTO.PublisherID))
            {
                ModelState.AddModelError(nameof(addBookRequestDTO.PublisherID),
                    $"PublisherID {addBookRequestDTO.PublisherID} does not exist");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}