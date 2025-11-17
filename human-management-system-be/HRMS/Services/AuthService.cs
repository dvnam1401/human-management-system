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

    public async Task<LoginResponseDto?> RegisterAsync(CreateRegisterDto dto)
    {
        try
        {
            var existingUser = await _userRepository.GetByUsernameOrEmailAsync(dto.Username);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration failed: Username already exists - {Username}", dto.Username);
                return null;
            }

            var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
            {
                _logger.LogWarning("Registration failed: Email already exists - {Email}", dto.Email);
                return null;
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new HRMS.Model1s.User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = passwordHash,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Role = dto.Role ?? "Employee",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            var createdUser = await _userRepository.CreateAsync(user);

            _logger.LogInformation("User registered successfully - {Username}", createdUser.Username);

            return new LoginResponseDto
            {
                UserId = createdUser.Id,
                Username = createdUser.Username,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Role = createdUser.Role,
                IsActive = createdUser.IsActive ?? true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Username}", dto.Username);
            return null;
        }
    }
}

