using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using human_management_system_fe.Services;

namespace human_management_system_fe.Pages.Auth
{
    public class LogoutModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(IApiService apiService, ILogger<LogoutModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Logout from backend (optional, to clean up backend session/cookie if any)
                await _apiService.LogoutAsync();

                // Logout from frontend
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                _logger.LogInformation("User logged out successfully");

                return RedirectToPage("/Auth/Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToPage("/Auth/Login");
            }
        }
    }
}

