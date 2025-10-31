using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using human_management_system_fe.Models;
using human_management_system_fe.Services;

namespace human_management_system_fe.Pages.Email
{
    [Authorize]
    public class SendModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<SendModel> _logger;

        [BindProperty]
        public SendEmailDto EmailInput { get; set; } = new SendEmailDto();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public SendModel(IApiService apiService, ILogger<SendModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.SendEmailAsync(EmailInput);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Không thể kết nối đến email service");
                    return Page();
                }

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return Page();
                }

                SuccessMessage = "Email đã được gửi thành công!";
                _logger.LogInformation("Email sent successfully to {Recipient}", EmailInput.RecipientEmail);
                
                // Redirect để clear form
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi gửi email. Vui lòng thử lại.");
                return Page();
            }
        }
    }
}

