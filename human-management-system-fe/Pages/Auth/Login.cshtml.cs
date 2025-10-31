using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using human_management_system_fe.Models;
using human_management_system_fe.Services;

namespace human_management_system_fe.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public LoginDto LoginInput { get; set; } = new LoginDto();

        [TempData]
        public string? ErrorMessage { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        public LoginModel(IApiService apiService, ILogger<LoginModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Nếu đã đăng nhập, redirect về trang chủ
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Gọi Backend API để verify credentials
                var result = await _apiService.LoginAsync(LoginInput);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Username/Email hoặc Password không đúng");
                    return Page();
                }

                // Tạo claims cho Frontend cookie authentication
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
                    IsPersistent = LoginInput.RememberMe,
                    ExpiresUtc = LoginInput.RememberMe
                        ? DateTimeOffset.UtcNow.AddDays(30)
                        : DateTimeOffset.UtcNow.AddHours(8)
                };

                // Sign in trên Frontend
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                _logger.LogInformation("User {Username} logged in successfully", result.Username);

                SuccessMessage = "Đăng nhập thành công!";
                return LocalRedirect(returnUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi đăng nhập. Vui lòng thử lại.");
                return Page();
            }
        }
    }
}

