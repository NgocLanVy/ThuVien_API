using Microsoft.AspNetCore.Mvc;
using WebAPI_simple.Data;
using WebAPI_simple.Models.DTO;
using WebAPI_simple.Repositories;

namespace WebAPI_simple.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(AppDbContext dbContext, IAuthorRepository authorRepository)
        {
            _dbContext = dbContext;
            _authorRepository = authorRepository;
        }

        [HttpGet("get-all-author")]
        public IActionResult GetAllAuthor([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] string? sortBy, [FromQuery] bool isAscending = true,[FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var allAuthors = _authorRepository.GetAllAuthors(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);
            return Ok(allAuthors);
        }

        [HttpGet("get-author-by-id/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var authorWithId = _authorRepository.GetAuthorById(id);
            if (authorWithId == null)
            {
                return NotFound(new { message = "Không tìm thấy tác giả" });
            }
            return Ok(authorWithId);
        }

        [HttpPost("add-author")]
        public IActionResult AddAuthor([FromBody] AddAuthorRequestDTO addAuthorRequestDTO)
        {
            if (!ValidateAuthorName(addAuthorRequestDTO?.FullName))
            {
                return BadRequest(ModelState);
            }

            var authorAdd = _authorRepository.AddAuthor(addAuthorRequestDTO);
            return Ok(authorAdd);
        }

        [HttpPut("update-author-by-id/{id}")]
        public IActionResult UpdateAuthorById(int id, [FromBody] AuthorNoIdDTO authorDTO)
        {
            if (!ValidateAuthorName(authorDTO?.FullName))
            {
                return BadRequest(ModelState);
            }

            var authorUpdate = _authorRepository.UpdateAuthorById(id, authorDTO);
            return Ok(authorUpdate);
        }

        [HttpDelete("delete-author-by-id/{id}")]
        public IActionResult DeleteAuthorById(int id)
        {
            var authorDelete = _authorRepository.DeleteAuthorById(id);
            return Ok(authorDelete);
        }
        [HttpGet("{id}/books")]
        public IActionResult GetBooksByAuthorId(int id)
        {
            var result = _authorRepository.GetBooksByAuthorId(id);
            if (result == null)
            {
                return NotFound(new { message = "Không tìm thấy tác giả" });
            }
            return Ok(result);
        }
        #region Private methods
        private bool ValidateAuthorName(string? fullName)
        {
            //2.kh đc để trống
            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError("FullName", "FullName cannot be empty");
            }
            //2.độ dài tối thiểu 3 ký tự
            else if (fullName.Trim().Length < 3)
            {
                ModelState.AddModelError("FullName", "FullName must be at least 3 characters");
            }

            return ModelState.ErrorCount == 0;
        }
        #endregion
    }
}