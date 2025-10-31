using HRMS.DTOs;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, ILogger<AccountController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Lấy thông tin cá nhân của user hiện tại (yêu cầu authentication)
    /// </summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Không tìm thấy thông tin user" });
            }

            var profile = await _userService.GetUserProfileAsync(userId);
            
            if (profile == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin user" });
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile");
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy thông tin cá nhân" });
        }
    }

    /// <summary>
    /// Lấy thông tin user theo userId (không yêu cầu authentication - cho internal calls)
    /// </summary>
    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<UserProfileDto>> GetProfileById(int userId)
    {
        try
        {
            var profile = await _userService.GetUserProfileAsync(userId);
            
            if (profile == null)
            {
                return NotFound(new { message = "Không tìm thấy thông tin user" });
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile by id");
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy thông tin cá nhân" });
        }
    }

    /// <summary>
    /// Yêu cầu reset password - gửi OTP qua email
    /// </summary>
    [HttpPost("reset-password/request")]
    public async Task<ActionResult> RequestPasswordReset([FromBody] ResetPasswordRequestDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.RequestPasswordResetAsync(dto.Email);

            if (!result)
            {
                return BadRequest(new { message = "Không thể gửi OTP. Vui lòng thử lại sau." });
            }

            return Ok(new { message = "Mã OTP đã được gửi đến email của bạn" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting password reset");
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi yêu cầu reset password" });
        }
    }

    /// <summary>
    /// Xác nhận OTP và đổi password
    /// </summary>
    [HttpPost("reset-password/verify")]
    public async Task<ActionResult> VerifyOtpAndResetPassword([FromBody] VerifyOtpDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.ResetPasswordWithOtpAsync(dto);

            if (!result)
            {
                return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn" });
            }

            return Ok(new { message = "Password đã được reset thành công" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying OTP and resetting password");
            return StatusCode(500, new { message = "Đã xảy ra lỗi khi reset password" });
        }
    }
}

