using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using human_management_system_fe.Models;
using human_management_system_fe.Services;

namespace human_management_system_fe.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<RegisterModel> _logger;

        [BindProperty]
        public RegisterDto RegisterInput { get; set; } = new RegisterDto();

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        public RegisterModel(IApiService apiService, ILogger<RegisterModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.RegisterAsync(RegisterInput);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Username hoặc Email đã tồn tại. Vui lòng thử lại.");
                    return Page();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                    new Claim(ClaimTypes.Name, result.Username),
                    new Claim(ClaimTypes.Email, result.Email),
                    new Claim(ClaimTypes.GivenName, result.FirstName ?? ""),
                    new Claim(ClaimTypes.Surname, result.LastName ?? ""),
                    new Claim(ClaimTypes.Role, result.Role ?? "Employee")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                _logger.LogInformation("User {Username} registered and logged in successfully", result.Username);

                SuccessMessage = "Đăng ký thành công! Bạn đã được đăng nhập tự động.";
                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đăng ký. Vui lòng thử lại.");
                return Page();
            }
        }
    }
}

