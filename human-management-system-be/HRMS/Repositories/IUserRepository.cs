using HRMS.Model1s;

namespace HRMS.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameOrEmailAsync(string identifier);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> UpdatePasswordAsync(int userId, string passwordHash);
    Task<User?> GetUserWithEmployeeDetailsAsync(int userId);
    Task<User> CreateAsync(User user);
}

