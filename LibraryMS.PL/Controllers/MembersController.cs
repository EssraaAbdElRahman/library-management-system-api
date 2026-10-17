using LibraryMS.BLL.DTOs.Member;
namespace LibraryMS.PL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Librarian")]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _memberService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Search(
            [FromQuery] string? name,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _memberService.SearchAsync(name, isActive, page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMemberDto dto)
        {
            var result = await _memberService.CreateAsync(dto, CurrentUserId);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberDto dto)
        {
            var result = await _memberService.UpdateAsync(id, dto, CurrentUserId);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _memberService.DeleteAsync(id, CurrentUserId);
            return NoContent();
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _memberService.ActivateAsync(id, CurrentUserId);
            return Ok(result);
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _memberService.DeactivateAsync(id, CurrentUserId);
            return Ok(result);
        }
    }
}
