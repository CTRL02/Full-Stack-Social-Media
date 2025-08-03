using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<ImpressionsController> _localizer;

        public ImpressionsController(ImpressionService service, IStringLocalizer<ImpressionsController> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        [HttpPost("leaveImpression")]
        [Authorize]
        public async Task<IActionResult> ToggleImpression([FromBody] ImpressionDto dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            try
            {
                var result = await _service.ToggleImpressionAsync(userId, dto);
                var message = result ? _localizer["ImpressionAdded"].Value
                                  : _localizer["ImpressionRemoved"].Value;
                return Ok(new { message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message switch
                    {
                        "InvalidImpressionType" => _localizer["InvalidImpressionType"].Value,
                        "TargetIdRequired" => _localizer["TargetIdRequired"].Value,
                        "SingleTargetRequired" => _localizer["SingleTargetRequired"].Value,
                        _ => ex.Message
                    }
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { error = _localizer["TargetNotFound"].Value });
            }
          
        }
    }
}
