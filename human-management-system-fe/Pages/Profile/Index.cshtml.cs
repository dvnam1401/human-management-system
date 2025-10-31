using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using human_management_system_fe.Models;
using human_management_system_fe.Services;

namespace human_management_system_fe.Pages.Profile
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

        public UserProfileDto? Profile { get; set; }
        public string? ErrorMessage { get; set; }

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy userId từ claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    _logger.LogWarning("User ID claim not found or invalid");
                    return RedirectToPage("/Auth/Login");
                }

                // Gọi API backend để lấy profile
                Profile = await _apiService.GetUserProfileAsync(userId);

                if (Profile == null)
                {
                    ErrorMessage = "Không thể tải thông tin cá nhân";
                    _logger.LogWarning("Failed to load profile for user {UserId}", userId);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading profile page");
                ErrorMessage = "Đã xảy ra lỗi khi tải thông tin cá nhân";
                return Page();
            }
        }
    }
}

