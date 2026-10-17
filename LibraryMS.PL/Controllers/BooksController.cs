using LibraryMS.BLL.DTOs.Book;
namespace LibraryMS.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? title,
            [FromQuery] int? categoryId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bookService.SearchAsync(title, categoryId, page, pageSize);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookDto dto)
        {
            var result = await _bookService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            var result = await _bookService.UpdateAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id, CurrentUserId);
            return NoContent();
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("{id}/copies/add")]
        public async Task<IActionResult> AddCopies(int id, [FromBody] AddBookCopiesDto dto)
        {
            var result = await _bookService.AddCopiesAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("{id}/copies/remove")]
        public async Task<IActionResult> RemoveCopies(int id, [FromBody] RemoveBookCopiesDto dto)
        {
            var result = await _bookService.RemoveCopiesAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost("{id}/authors")]
        public async Task<IActionResult> AddAuthor(int id, [FromBody] AddAuthorToBookDto dto)
        {
            var result = await _bookService.AddAuthorAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpDelete("{id}/authors/{authorId}")]
        public async Task<IActionResult> RemoveAuthor(int id, int authorId)
        {
            var result = await _bookService.RemoveAuthorAsync(id, authorId, CurrentUserId);
            return Ok(result);
        }
    }
}