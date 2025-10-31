using HRMS.DTOs;
using HRMS.Repositories;
using BCrypt.Net;

namespace HRMS.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await _userRepository.GetByUsernameOrEmailAsync(loginDto.UsernameOrEmail);
            
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found - {Identifier}", loginDto.UsernameOrEmail);
                return null;
            }

            if (user.IsActive == false)
            {
                _logger.LogWarning("Login failed: User is inactive - {Identifier}", loginDto.UsernameOrEmail);
                return null;
            }

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Invalid password - {Identifier}", loginDto.UsernameOrEmail);
                return null;
            }

            _logger.LogInformation("User logged in successfully - {Username}", user.Username);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                IsActive = user.IsActive ?? true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Identifier}", loginDto.UsernameOrEmail);
            return null;
        }
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameOrEmailAsync(username);
        
        if (user == null || user.IsActive == false)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}

