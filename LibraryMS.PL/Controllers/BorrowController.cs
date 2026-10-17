using LibraryMS.BLL.DTOs.BorrowRecord;
namespace LibraryMS.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Librarian")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _borrowService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet("member/{memberId}")]
        public async Task<IActionResult> GetByMember(int memberId)
        {
            var result = await _borrowService.GetByMemberAsync(memberId);
            return Ok(result);
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdue()
        {
            var result = await _borrowService.GetOverdueAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Borrow([FromBody] CreateBorrowDto dto)
        {
            var result = await _borrowService.BorrowBookAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("{id}/return")]
        public async Task<IActionResult> Return(int id, [FromBody] ReturnBookDto dto)
        {
            var result = await _borrowService.ReturnBookAsync(id, dto, CurrentUserId);
            return Ok(result);
        }
    }
}