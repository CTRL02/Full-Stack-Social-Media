using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using socialmedia.DTOs;
using socialmedia.Repositories.ImpressionService;
using System.Security.Claims;

namespace socialmedia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImpressionsController : ControllerBase
    {
        private readonly ImpressionService _service;

        public ImpressionsController(ImpressionService service)
        {
            _service = service;
        }

        [HttpPost("leaveImpression")]
        [Authorize]
        public async Task<IActionResult> ToggleImpression([FromBody] ImpressionDto dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var message = await _service.ToggleImpressionAsync(userId, dto);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
