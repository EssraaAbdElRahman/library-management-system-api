using LibraryMS.BLL.DTOs.Author;

namespace LibraryMS.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _authorService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            var result = await _authorService.SearchByNameAsync(name);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAuthorDto dto)
        {
            var result = await _authorService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
        {
            var result = await _authorService.UpdateAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.DeleteAsync(id, CurrentUserId);
            return NoContent();
        }
    }
}
