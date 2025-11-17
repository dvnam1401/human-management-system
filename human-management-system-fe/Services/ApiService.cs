using human_management_system_fe.Models;
using System.Net.Http.Json;

namespace human_management_system_fe.Services;

public class ApiService : IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ApiService> _logger;

    public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HRMS_API");
            var response = await client.PostAsJsonAsync("/api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }

            _logger.LogWarning("Login failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling login API");
            return null;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HRMS_API");
            var response = await client.PostAsync("/api/auth/logout", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling logout API");
            return false;
        }
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(int userId)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HRMS_API");
            
            // Gọi API backend để lấy profile theo userId
            // Sử dụng endpoint không yêu cầu authentication cho server-to-server call
            var response = await client.GetAsync($"/api/account/profile/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserProfileDto>();
            }

            _logger.LogWarning("Get profile failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling get profile API");
            return null;
        }
    }

    public async Task<EmailResponseDto?> SendEmailAsync(SendEmailDto emailDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HRMS_API");
            
            // Gọi API backend để gửi email
            var response = await client.PostAsJsonAsync("/api/email/send", emailDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<EmailResponseDto>();
            }

            // Nếu không thành công, thử parse error message
            try
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<EmailResponseDto>();
                return errorResponse;
            }
            catch
            {
                _logger.LogWarning("Send email failed with status code: {StatusCode}", response.StatusCode);
                return new EmailResponseDto
                {
                    Success = false,
                    Message = "Không thể gửi email"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling send email API");
            return new EmailResponseDto
            {
                Success = false,
                Message = "Đã xảy ra lỗi khi gửi email"
            };
        }
    }

    public async Task<LoginResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("HRMS_API");
            var response = await client.PostAsJsonAsync("/api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }

            _logger.LogWarning("Register failed with status code: {StatusCode}", response.StatusCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling register API");
            return null;
        }
    }
}

